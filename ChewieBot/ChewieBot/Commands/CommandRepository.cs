using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.ScriptingEngine;
using System.Configuration;
using ChewieBot.Exceptions;
using ChewieBot.Constants;
using ChewieBot.Services;
using ChewieBot.AppStart;
using ChewieBot.Events.TwitchPubSub;
using ChewieBot.Events;

namespace ChewieBot.Commands
{
    public class CommandRepository : ICommandRepository
    {
        private Dictionary<string, Command> commands;
        private Dictionary<string, List<Command>> eventMappedCommands;
        private IPythonEngine scriptEngine;

        public CommandRepository(IPythonEngine scriptEngine)
        {
            this.commands = new Dictionary<string, Command>();
            this.eventMappedCommands = new Dictionary<string, List<Command>>();
            this.scriptEngine = scriptEngine;
        }

        public List<Command> GetAllCommands()
        {
            return this.commands.Values.ToList();
        }

        public void LoadCommands()
        {
            var commandsPath = ConfigurationManager.AppSettings["CommandsPath"];
            this.commands = this.scriptEngine.LoadScripts(commandsPath);
            this.RegisterEventCommands();
        }

        private void RegisterEventCommands()
        {
            foreach (var command in this.commands.Values.Where(x => x.IsEventTriggered).ToList())
            {
                foreach (var eventToRegister in command.EventsToRegister)
                {
                    var eventClassName = eventToRegister.Split('.')[0];
                    var eventName = eventToRegister.Split('.')[1];
                    // Need the full name of the class that the event is in, including assembly, to get the type.
                    var type = Type.GetType($"{AppConstants.ServiceAssmebly.Path}.{eventClassName}");
                    var interfaceType = Type.GetType($"{AppConstants.ServiceAssmebly.InterfacePath}.I{eventClassName}");
                    var eventInfo = type.GetEvent(eventName);
                    var eventHandlerType = eventInfo.EventHandlerType;

                    // Need to use generics as the events that scripts can register to have different eventArgs.
                    // GenerticTypeArguments gets the type of the eventArgs needed, then we pass that to the MakeGeneric to make the correct method to bind to the event.
                    var eventArgsType = eventHandlerType.GenericTypeArguments[0];
                    var methodInfo = this.GetType().GetMethod("ExecuteEventCommand").MakeGenericMethod(eventArgsType);
                    var del = Delegate.CreateDelegate(eventHandlerType, this, methodInfo);

                    var eventService = UnityConfig.ResolveByType(interfaceType);

                    eventInfo.AddEventHandler(eventService, del);

                    // Add the command to list of commands registered to this event.
                    if (this.eventMappedCommands.Keys.Any(x => x == eventInfo.Name))
                    {
                        this.eventMappedCommands[eventInfo.Name].Add(command);
                    }
                    else
                    {
                        this.eventMappedCommands.Add(eventInfo.Name, new List<Command>() { command });
                    }
                }
            }
        }

        public void ExecuteEventCommand<T>(object sender, T args)
            where T : BaseEvent
        {
            var eventName = args.TriggeredByEvent;
            if (this.eventMappedCommands.Keys.Any(x => x == eventName))
            {
                // Iterate through all commands registered to this event and execute them.
                var commandList = this.eventMappedCommands[eventName];
                foreach (var command in commandList)
                {
                    this.scriptEngine.ExecuteEventCommand(command, new EventScriptInfo { EventName = eventName });
                }
            }
        }

        public void ExecuteCommand(string commandName, string username, List<string> chatParameters) 
        {

            var command = this.GetCommand(commandName);
            if (command == null)
            {
                throw new CommandNotExistException($"{commandName} is not a valid command.");
            }

            if (command.Parameters != null && chatParameters.Count > 0)
            {
                var requiredParams = command.Parameters.Where(x => x.IsRequired);
                if (command.Parameters.Count != chatParameters.Count && requiredParams.Count() != chatParameters.Count)
                {
                    // TODO: Ignore excess parameters or return an error?
                    // For now, just remove excess parameters passed
                    var count = chatParameters.Count > command.Parameters.Count 
                        ? chatParameters.Count - command.Parameters.Count 
                        : command.Parameters.Count - chatParameters.Count;
                    chatParameters.RemoveRange(command.Parameters.Count, count);
                }

                if (chatParameters != null && chatParameters.Count > 0)
                {
                    this.scriptEngine.ExecuteCommand(command, username, chatParameters);
                }
            }
            else
            {
                this.scriptEngine.ExecuteCommand(command, username);
            }
        }

        public Command GetCommand(string commandName)
        {
            if (!this.commands.ContainsKey(commandName))
            {
                return null;
            }

            return this.commands[commandName];
        }
    }
}

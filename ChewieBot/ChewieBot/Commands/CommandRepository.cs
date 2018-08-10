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

namespace ChewieBot.Commands
{
    public class CommandRepository : ICommandRepository
    {
        private Dictionary<string, Command> commands;
        private IPythonEngine scriptEngine;

        public CommandRepository(IPythonEngine scriptEngine)
        {
            this.commands = new Dictionary<string, Command>();
            this.scriptEngine = scriptEngine;
            this.LoadCommands();
        }

        public List<Command> GetAllCommands()
        {
            return this.commands.Values.ToList();
        }

        public void LoadCommands()
        {
            var commandsPath = ConfigurationManager.AppSettings["CommandsPath"];
            this.commands = this.scriptEngine.LoadScripts(commandsPath);
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

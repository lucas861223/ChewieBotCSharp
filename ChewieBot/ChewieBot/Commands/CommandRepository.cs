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
            if (!this.commands.ContainsKey(commandName))
            {
                throw new CommandNotExistException($"{commandName} is not a valid command.");
            }

            if (this.commands[commandName].Parameters.Count != chatParameters.Count)
            {
                
            }

            if (chatParameters != null && chatParameters.Count > 0)
            {
                this.scriptEngine.ExecuteCommand(this.commands[commandName], username, chatParameters);
            }
            else
            {
                this.scriptEngine.ExecuteCommand(this.commands[commandName], username);
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.ScriptingEngine;

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

        public void LoadCommands()
        {
            this.commands = this.scriptEngine.LoadScripts();
        }

        public void ExecuteCommand(string commandName, string username, List<string> chatParameters) 
        {
            if (this.commands.ContainsKey(commandName))
            {
                if (chatParameters != null && chatParameters.Count > 0)
                {
                    this.scriptEngine.ExecuteCommand(this.commands[commandName], username, chatParameters);
                }
                else
                {
                    this.scriptEngine.ExecuteCommand(this.commands[commandName], username);
                }
            }
        }
    }
}

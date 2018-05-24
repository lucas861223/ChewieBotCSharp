using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Scripting;
using ChewieBot.Database.Model;
using Microsoft.ClearScript.V8;

namespace ChewieBot.Commands
{
    public class CommandRepository : ICommandRepository
    {
        private Dictionary<string, Command> commands;
        private IScriptEngine scriptEngine;

        public CommandRepository(IScriptEngine scriptEngine)
        {
            this.commands = new Dictionary<string, Command>();
            this.scriptEngine = scriptEngine;
            this.LoadCommands();
        }

        public void LoadCommands()
        {
            this.commands = this.scriptEngine.LoadScripts();
        }

        public CommandResponse ExecuteCommand(string commandName, string username, List<string> chatParameters) 
        {
            if (this.commands.ContainsKey(commandName))
            {
                var response = this.scriptEngine.ExecuteCommand(this.commands[commandName], username, chatParameters);
                return response;
            }
            else
            {
                return null;
            }
        }
    }
}

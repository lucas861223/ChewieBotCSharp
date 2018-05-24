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
    public class CommandRepository
    {
        private List<string> commands;
        private ScriptEngine scriptEngine;

        public CommandRepository()
        {
            this.commands = new List<string>();
            this.scriptEngine = new ScriptEngine();
            this.LoadCommands();
        }

        public void LoadCommands()
        {
            this.commands = this.scriptEngine.LoadScripts();
        }

        public CommandResponse ExecuteCommand(string commandName, string username, dynamic parameters) 
        {
            if (this.commands.Contains(commandName))
            {
                var response = this.scriptEngine.ExecuteScript(commandName, username, parameters);
                return response;
            }
            else
            {
                return null;
            }
        }
    }
}

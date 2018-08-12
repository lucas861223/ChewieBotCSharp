using ChewieBot.Commands;
using ChewieBot.ScriptingEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ViewModels
{
    public class CommandEditorViewModel
    {
        public string CommandName { get; set; }
        public string CommandSource { get; set; }

        public CommandEditorViewModel(string commandName, string commandSource)
        {
            this.CommandName = commandName;
            this.CommandSource = commandSource;
        }
    }
}

using ChewieBot.Commands;
using ChewieBot.Models;
using ChewieBot.ScriptingEngine;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ViewModels
{
    public class CommandListViewModel
    {
        private ICommandRepository commandRepo;
        public List<CommandListItem> CommandList { get; set; }

        public CommandListViewModel(ICommandRepository commandRepo)
        {
            this.commandRepo = commandRepo;
            this.CommandList = this.commandRepo.GetAllCommands().Select(x => new CommandListItem { CommandName = x.CommandName, Parameters = x.Parameters, Cost = x.PointCost }).ToList();
        }
    }
}

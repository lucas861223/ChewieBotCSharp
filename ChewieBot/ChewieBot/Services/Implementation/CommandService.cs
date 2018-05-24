using ChewieBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class CommandService : ICommandService
    {
        private ICommandRepository commandRepository;

        public CommandService(ICommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public CommandResponse ExecuteCommand(string commandName, string username, List<string> chatParameters = null) 
        {
            var response = this.commandRepository.ExecuteCommand(commandName, username, chatParameters);
            return response;
        }
    }
}

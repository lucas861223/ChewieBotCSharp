using ChewieBot.Commands;
using ChewieBot.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class CommandService : ICommandService
    {
        private CommandRepository commandRepository;

        public CommandService(IUserService userService)
        {
            this.commandRepository = new CommandRepository();
        }

        public CommandResponse ExecuteCommand(string commandName, string username, List<string> chatParameters = null) 
        {
            var response = this.commandRepository.ExecuteCommand(commandName, username, chatParameters);
            return response;
        }
    }
}

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
        private ITwitchService twitchService;
        private IUserService userService;
        private CommandRepository commandRepository;

        public CommandService(IUserService userService)
        {
            //this.twitchService = twitchService;
            this.userService = userService;
            this.commandRepository = new CommandRepository();
        }

        public void ExecuteCommand(string commandName, string username, dynamic parameters = null) 
        {
            var response = this.commandRepository.ExecuteCommand(commandName, username, parameters);
            if (response != null)
            {
                Console.WriteLine(response);
            }
        }
    }
}

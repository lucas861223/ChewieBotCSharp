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

        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="commandName">The name for the command. This is the string immediately after the command identifier - for the command !addpoints user 10, the commandName is addpoints</param>
        /// <param name="username">The user who is executing the command.</param>
        /// <param name="chatParameters">The parameters passed to the command from the message. This is a list of space separated words from the same message as the command 
        /// - for the command !addpoints user 10, the chat parameters are 'user' and '10'.</param>
        /// <returns>A CommandResponse for the executed command, containing the status and a message if the command returns a message.</returns>
        public CommandResponse ExecuteCommand(string commandName, string username, List<string> chatParameters = null) 
        {
            var response = this.commandRepository.ExecuteCommand(commandName, username, chatParameters);
            return response;
        }
    }
}

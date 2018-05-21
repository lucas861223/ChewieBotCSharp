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

        private Queue<ICommand> commandQueue;
        private CommandRepository commandRepository;

        public event EventHandler<CommandExecutedEventArgs> CommandExecutedEvent;

        public CommandService(ITwitchService twitchService)
        {
            this.twitchService = twitchService;
            this.commandQueue = new Queue<ICommand>();
            this.commandRepository = new CommandRepository();
            this.commandRepository.LoadCommands();
        }

        public void QueueCommand(ICommand command)
        {
            // TODO: Add buffering system to prevent executing commands until after time has passed since adding a new command.
            this.commandQueue.Enqueue(command);
        }

        private void ExecuteCommands()
        {
            // TODO: merge commands that are the same if possible, (e.g. !points could return everyones query in a single reply)
            while (this.commandQueue.Count > 0)
            {
                var command = this.commandQueue.Dequeue();
                command.Execute();
                this.CommandExecutedEvent?.Invoke(this, new CommandExecutedEventArgs(command));
            }
        }
    }
}

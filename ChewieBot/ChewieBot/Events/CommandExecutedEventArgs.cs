using ChewieBot.Commands;

namespace ChewieBot.Events
{
    public class CommandExecutedEventArgs
    {
        public ICommand Command { get; private set; }

        public CommandExecutedEventArgs(ICommand command)
        {
            this.Command = command;
        }
    }
}
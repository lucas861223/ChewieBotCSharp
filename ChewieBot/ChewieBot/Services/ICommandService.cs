using ChewieBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface ICommandService
    {
        void ExecuteCommand(string commandName, string username, List<string> parameters = null);
        void Initialize();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Commands;

namespace ChewieBot.Scripting
{
    public interface IPythonEngine
    {
        Dictionary<string, Command> LoadScripts();
        CommandResponse ExecuteCommand(Command commandName, string username, List<string> chatParameters);
        CommandResponse ExecuteCommand(Command commandName, string username);
    }
}

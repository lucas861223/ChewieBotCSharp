using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ScriptingEngine
{
    public interface IPythonEngine
    {
        Dictionary<string, Command> LoadScripts(string commandPaths);
        void ExecuteCommand(Command commandName, string username, List<string> chatParameters);
        void ExecuteCommand(Command commandName, string username);
    }
}

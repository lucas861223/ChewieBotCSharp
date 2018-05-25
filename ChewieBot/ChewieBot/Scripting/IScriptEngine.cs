using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Commands;

namespace ChewieBot.Scripting
{
    public interface IScriptEngine
    {
        Dictionary<string, Command> LoadScripts();
        CommandResponse ExecuteCommand(Command commandName, string username, List<string> chatParameters);
        void TestPython(string path, string username, dynamic parameters = null);
    }
}

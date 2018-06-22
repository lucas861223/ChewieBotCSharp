using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;

namespace ChewieBot.ScriptingEngine
{
    public class Command
    {
        public List<string> Parameters { get; set; }
        public string CommandName { get; set; }
        public ScriptSource Source { get; set; }
    }
}

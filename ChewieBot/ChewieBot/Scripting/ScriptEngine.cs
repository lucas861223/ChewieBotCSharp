using ChewieBot.Services.Implementation;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Scripting
{
    public class ScriptEngine
    {
        private V8ScriptEngine engine;

        public ScriptEngine()
        {
            this.engine = new V8ScriptEngine();
            this.ExposeAPIs();

        }

        private void ExposeAPIs()
        {
            this.engine.AddHostType("UserService", typeof(UserService));
            this.engine.AddHostType("Console", typeof(Console));
        }

        public void ExecuteScript(string scriptPath)
        {
            var script = File.ReadAllText(scriptPath);
            this.engine.Execute(script);
            var result = this.engine.Script.result;
            Console.WriteLine(result);
        }
    }
}

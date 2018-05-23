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
        private object transpileTsc;

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
            TranspileTypescript("test.ts");
        }

        private string TranspileTypescript(string typescriptScriptPath)
        {
            var tsScript = File.ReadAllText(typescriptScriptPath);
            dynamic tsc = this.engine.Evaluate(File.ReadAllText("scripting/typescriptTranspiler.js"));
            dynamic tscTranspile = this.engine.Evaluate("ts.transpile");
            var test = tscTranspile(tsScript);
            var testJS = this.engine.Evaluate(test);
            dynamic result = this.engine.Evaluate("new Test()");
            result.execute();
            return "Test";
        }
    }
}

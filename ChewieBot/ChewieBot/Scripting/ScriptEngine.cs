using ChewieBot.Services.Implementation;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Commands;
using ChewieBot.Database.Model;
using ChewieBot.AppStart;
using ChewieBot.Services;
using ChewieBot.Scripting.Services;

namespace ChewieBot.Scripting
{
    public class ScriptEngine
    {
        private V8ScriptEngine engine;
        private dynamic transpileTsc;

        public ScriptEngine()
        {
            this.engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging | V8ScriptEngineFlags.EnableRemoteDebugging, 9222);

            this.CreateTranspiler();            
            this.ExposeAPIs();
        }

        private void ExposeAPIs()
        {
            this.ExposeServices();
            this.ExposeModels();
        }

        private void ExposeServices()
        {
            this.engine.AddHostObject("UserService", UnityConfig.Resolve<ScriptUserService>());
            this.engine.AddHostType("Console", typeof(Console));
        }

        private void ExposeModels()
        {
            this.engine.AddHostType("User", typeof(User));
            this.engine.AddHostType("CommandResponse", typeof(CommandResponse));
        }

        public List<string> LoadScripts()
        {
            var list = new List<string>();
            foreach (var file in Directory.EnumerateFiles("Commands\\CommandScripts"))
            {
                var fileName = file.Split('\\').Last();
                fileName = fileName.Substring(0, fileName.Length - 3);
                var chatCommandName = fileName.Substring(0, fileName.IndexOf("Command"));
                if (this.TranspileTypescript(chatCommandName, file))
                {
                    list.Add(chatCommandName);
                }
            }
            return list;
        }

        public CommandResponse ExecuteScript(string command, string username, dynamic parameters)
        {
            if (this.engine.Script.parameters == null)
            {
                this.engine.AddHostObject("parameters", parameters);
            }
            else
            {
                this.engine.Script.parameters = parameters;
            }

            var response = this.engine.Evaluate($"{command}.execute('{username}', parameters)");
            return (CommandResponse)response;
        }

        /// <summary>
        /// Transpiles a TypeScript file to a Javascript file, then adds it to the scripting engine to be used.
        /// </summary>
        /// <param name="commandName">Variable name that will be assigned in the scripting engine to the javascript onject of the loaded file.</param>
        /// <param name="commandScriptPath">The path to the Typescript file to load in to the scripting engine. The Typescript file name needs to be in the format {commandName}Command.ts</param>
        /// <returns></returns>
        private bool TranspileTypescript(string commandName, string commandScriptPath)
        {
            if (File.Exists(commandScriptPath) && commandScriptPath.Substring(commandScriptPath.Length - 3) == ".ts")
            {
                var tsScript = File.ReadAllText(commandScriptPath);
                var script = this.transpileTsc(tsScript);
                this.engine.Evaluate(script);
                var scriptCommandName = commandName.First().ToString().ToUpper() + commandName.Substring(1) + "Command";
                this.engine.Evaluate($"let {commandName} = new {scriptCommandName}()");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the Typescript Transpiler in to the engine so that we can transpile TypeScript files to Javascript.
        /// </summary>
        private void CreateTranspiler()
        {
            if (this.transpileTsc == null) {
                var tscScript = this.engine.Evaluate(File.ReadAllText("scripting/typescriptTranspiler.js"));
                this.transpileTsc = this.engine.Evaluate("ts.transpile");
            }
        }
    }
}

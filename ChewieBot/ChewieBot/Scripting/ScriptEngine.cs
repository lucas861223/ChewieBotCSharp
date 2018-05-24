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
using System.Dynamic;
using System.Timers;
using Microsoft.ClearScript;
using ChewieBot.Scripting.Events;
using ChewieBot.Enum;

namespace ChewieBot.Scripting
{
    public class ScriptEngine : IScriptEngine
    {
        private static V8ScriptEngine engine;
        private dynamic transpileTsc;

        public ScriptEngine()
        {
            if (engine == null)
            {
                engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging | V8ScriptEngineFlags.EnableRemoteDebugging, 9222);
            }

            this.CreateTranspiler();            
            this.ExposeAPIs();
        }

        /// <summary>
        /// Adding host objects and types to the script engine, so they're available in scripts.
        /// </summary>
        private void ExposeAPIs()
        {
            this.ExposeServices();
            this.ExposeModels();
        }

        /// <summary>
        /// Add services to the script engine so that they're available in scripts.
        /// </summary>
        private void ExposeServices()
        {
            engine.AddHostObject("UserService", UnityConfig.Resolve<ScriptUserService>());
            engine.AddHostObject("ChatEventService", UnityConfig.Resolve<ScriptChatEventService>());
            engine.AddHostType("Console", typeof(Console));
            engine.AddHostType("Timer", typeof(Timer));
            engine.AddHostObject("host", new HostFunctions());
        }

        /// <summary>
        /// Add models to the script engine so that they're available in scripts.
        /// </summary>
        private void ExposeModels()
        {
            engine.AddHostType("User", typeof(User));
            engine.AddHostType("CommandResponse", typeof(CommandResponse));
            engine.AddHostType("EventType", typeof(EventType));
            engine.AddHostType("ResponseType", typeof(ResponseType));
            engine.AddHostType("ChatEvent", typeof(ChatEvent));
            engine.AddHostType("EventWinner", typeof(EventWinner));
        }

        /// <summary>
        /// Load TS scripts from the Commands\\CommandScripts folder and adds them to the script engine.
        /// </summary>
        /// <returns>A dictionary containing commands with their command name as the key. The command name is what is used to trigger the command.</returns>
        public Dictionary<string, Command> LoadScripts()
        {
            var dict = new Dictionary<string, Command>();
            foreach (var file in Directory.EnumerateFiles("Commands\\CommandScripts"))
            {
                var fileName = file.Split('\\').Last();
                fileName = fileName.Substring(0, fileName.Length - 3);
                var chatCommandName = fileName.Substring(0, fileName.IndexOf("Command"));
                var command = this.TranspileTypescript(chatCommandName, file);
                if (command != null)
                {
                    dict.Add(chatCommandName, command);
                }
            }
            return dict;
        }

        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="command">The command to exeute.</param>
        /// <param name="username">The user calling the command.</param>
        /// <param name="chatParameters">Any parameters for the command.</param>
        /// <returns>A CommandResponse object with the response of the command execution.</returns>
        public CommandResponse ExecuteCommand(Command command, string username, List<string> chatParameters)
        {
            var response = engine.Evaluate(this.CreateCommandCall(command, username, chatParameters));
            return (CommandResponse)response;
        }

        /// <summary>
        /// Creates the string that will be evaluated by the Script Engine to execute the command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="username">The user calling the command.</param>
        /// <param name="chatParameters">Any parameters for the command.</param>
        /// <returns>A string that can be evaluated by the script engine to execute the command.</returns>
        private string CreateCommandCall(Command command, string username, List<string> chatParameters)
        {
            // Create a JSON object for the parameters, using the parameter names as the key, and the parameters from chat as theh value.
            var param = new StringBuilder();
            param.Append("{");
            for (int i = 0; i < chatParameters.Count(); i++)
            {
                if (i > 0)
                {
                    param.Append(",");
                }
                param.Append($"{command.Parameters[i]}: '{chatParameters[i]}'");
            }
            param.Append("}");

            var commandCall = $"{command.CommandName}.execute('{username}', {param.ToString()})";

            return commandCall;
        }

        /// <summary>
        /// Transpiles a TypeScript file to a Javascript file, then adds it to the scripting engine to be used.
        /// </summary>
        /// <param name="commandName">Variable name that will be assigned in the scripting engine to the javascript onject of the loaded file.</param>
        /// <param name="commandScriptPath">The path to the Typescript file to load in to the scripting engine. The Typescript file name needs to be in the format {commandName}Command.ts</param>
        /// <returns></returns>
        private Command TranspileTypescript(string commandName, string commandScriptPath)
        {
            if (File.Exists(commandScriptPath) && commandScriptPath.Substring(commandScriptPath.Length - 3) == ".ts")
            {
                var tsScript = File.ReadAllText(commandScriptPath);
                var scriptCommandName = commandName.First().ToString().ToUpper() + commandName.Substring(1) + "Command";

                // Checking to see if we've already added this command. If we have, skip it.
                dynamic engineCommand = engine.Script[scriptCommandName];
                if (engineCommand.GetType() == typeof(Microsoft.ClearScript.Undefined))
                {
                    // TODO: Should rework how parameters work so that we can get basic types (e.g. string, number, etc)
                    // as for now everything is a string, and we try to cast it in the services, returning errors if it fails.
                    var script = this.transpileTsc(tsScript);
                    engine.Evaluate(script);
                    // We need to create an object for the command so that we can use the execute function later, and so we can can get the parameters property
                    engine.Evaluate($"let {commandName} = new {scriptCommandName}()");                    

                    var command = new Command();
                    command.CommandName = commandName;
                    command.Parameters = this.GetCommandParameters(commandName);
                    return command;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a list of parameters names for a command.
        /// </summary>
        /// <param name="commandName">The command to get parameters for.</param>
        /// <returns>A list of parameter names.</returns>
        private List<string> GetCommandParameters(string commandName)
        {
            var list = new List<string>();

            // Check to see if we have any parameters
            dynamic parameters = engine.Evaluate($"{commandName}.parameters");
            if (parameters.GetType() != typeof(Microsoft.ClearScript.Undefined))
            {
                for (int i = 0; i < parameters.length; i++)
                {
                    list.Add(parameters[i]);
                }
            }

            return list;
        }

        /// <summary>
        /// Loads the Typescript Transpiler in to the engine so that we can transpile TypeScript files to Javascript.
        /// </summary>
        private void CreateTranspiler()
        {
            if (this.transpileTsc == null) {
                var tscScript = engine.Evaluate(File.ReadAllText("scripting/typescriptTranspiler.js"));
                this.transpileTsc = engine.Evaluate("ts.transpile");
            }
        }
    }
}

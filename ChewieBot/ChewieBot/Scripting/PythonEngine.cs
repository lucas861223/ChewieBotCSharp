using ChewieBot.Services.Implementation;
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
using ChewieBot.Scripting.Events;
using ChewieBot.Enum;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using IronPython.Runtime;

namespace ChewieBot.Scripting
{
    public class PythonEngine : IPythonEngine
    {
        private static ScriptEngine engine;

        public PythonEngine()
        {
            this.SetupPythonEngine();
        }

        private void SetupPythonEngine()
        {
            engine = Python.CreateEngine();
            var pc = HostingHelpers.GetLanguageContext(engine) as PythonContext;
            var hooks = pc.SystemState.Get__dict__()["path_hooks"] as List;
            hooks.Clear();

            var paths = engine.GetSearchPaths();
            paths.Add("..\\..\\..\\packages\\IronPython.StdLib.2.7.8.1\\contentFiles\\any\\any\\Lib");  // Adding stdlib
            engine.SetSearchPaths(paths);
        }

        private ScriptScope CreateScope()
        {
            var scope = engine.CreateScope();            
            scope.ImportModule("clr");
            engine.Execute("import clr", scope);
            engine.Execute("clr.AddReferenceToFileAndPath(\"ChewieBot.ScriptingAPI.dll\")", scope);
            engine.Execute("from ChewieBot.ScriptingAPI import *", scope);            
            return scope;
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
                var source = engine.CreateScriptSourceFromFile(file);                

                if (source != null)
                {
                    var command = new Command { CommandName = chatCommandName, Source = source, Parameters = GetCommandParameters(source) };
                    dict.Add(chatCommandName, command);
                }
            }
            return dict;
        }

        private List<string> GetCommandParameters(ScriptSource source)
        {
            var scope = this.CreateScope();
            source.Execute(scope);
            if (scope.ContainsVariable("parameters"))
            {
                var parameters = ((IList<object>)scope.GetVariable("parameters")).Cast<string>().ToList();
                return parameters;
            }

            return null;
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
            var scope = this.CreateScope();
            var paramsObject = this.CreateParamObject(command, chatParameters);
            command.Source.Execute(scope);
            var execute = scope.GetVariable<Func<string, dynamic, string>>("execute");
            var responseMessage = execute(username, paramsObject);

            return new CommandResponse(responseMessage);
        }

        public CommandResponse ExecuteCommand(Command command, string username)
        {
            var scope = this.CreateScope();
            command.Source.Execute(scope);
            var execute = scope.GetVariable<Func<string, string>>("execute");
            var responseMessage = execute(username);
            return new CommandResponse(responseMessage);
        }

        private dynamic CreateParamObject(Command command, List<string> chatParams)
        {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            if (chatParams.Count >= command.Parameters.Count)
            {
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    obj.Add(command.Parameters[i], chatParams[i]);
                }
            }

            return obj;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Dynamic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using IronPython.Runtime;
using ChewieBot.ScriptingEngine.Constants;

namespace ChewieBot.ScriptingEngine
{
    public class PythonEngine : IPythonEngine
    {
        private static ScriptEngine engine;

        public PythonEngine()
        {
            this.SetupPythonEngine();
        }

        /// <summary>
        /// Create the python engine.
        /// </summary>
        private void SetupPythonEngine()
        {
            engine = Python.CreateEngine();
            
            // Remove path_hooks to prevent python scripts throwing exceptions for zip imports.
            var pc = HostingHelpers.GetLanguageContext(engine) as PythonContext;
            var hooks = pc.SystemState.Get__dict__()["path_hooks"] as List;
            hooks.Clear();

            // Add the python stdlib to the paths so the stdlib is available in scripts.
            var paths = engine.GetSearchPaths();
            paths.Add("..\\..\\..\\packages\\IronPython.StdLib.2.7.8.1\\contentFiles\\any\\any\\Lib");
            engine.SetSearchPaths(paths);
        }

        /// <summary>
        /// Create a Python scope with the scripting API added as a reference and imported.
        /// </summary>
        /// <returns>ScriptScope with the scripting api added as a reference and imported.</returns>
        private ScriptScope CreateScope()
        {
            var scope = engine.CreateScope();            
            scope.ImportModule("clr");
            engine.Execute("import clr", scope);
            engine.Execute("clr.AddReferenceToFileAndPath(\"ChewieBot.ScriptingAPI.dll\")", scope);
            engine.Execute("from ChewieBot.ScriptingAPI.Services import *", scope);
            engine.Execute("from ChewieBot.ScriptingAPI.Enums import *", scope);
            return scope;
        }

        /// <summary>
        /// Load TS scripts from the Commands\\CommandScripts folder and adds them to the script engine.
        /// </summary>
        /// <returns>A dictionary containing commands with their command name as the key. The command name is what is used to trigger the command.</returns>
        public Dictionary<string, Command> LoadScripts(string commandsPath)
        {
            var dict = new Dictionary<string, Command>();
            foreach (var file in Directory.EnumerateFiles(commandsPath))
            {
                var fileName = file.Split('\\').Last();
                fileName = fileName.Substring(0, fileName.Length - 3);
                if (fileName.Contains("Command"))
                {
                    var chatCommandName = fileName.Substring(0, fileName.IndexOf("Command"));
                    var source = engine.CreateScriptSourceFromFile(file);
                    
                    if (source != null)
                    {
                        var command = this.CreateCommandObject(chatCommandName, source);
                        dict.Add(chatCommandName, command);
                    }
                }
            }
            return dict;
        }

        public Command LoadScript(string commandPath, string commandName)
        {
            if (Directory.EnumerateFiles(commandPath).Any(x => x == $"{commandPath}\\{commandName}Command.py"))
            {
                var source = engine.CreateScriptSourceFromFile($"{commandPath}\\{commandName}Command.py");
                if (source != null)
                {
                    var command = this.CreateCommandObject(commandName, source);
                    return command;
                }
            }
            return null;
        }

        private Command CreateCommandObject(string commandName, ScriptSource source)
        {
            var command = new Command();
            command.CommandName = commandName;
            command.Source = source;

            var scope = this.CreateScope();
            source.Execute(scope);
            command.Parameters = this.GetCommandParameters(scope);
            command.PointCost = this.GetCommandCost(scope);
            command.IsEventTriggered = this.IsEventTriggered(scope);
            command.EventsToRegister = this.GetEventsToRegister(scope);

            return command;
        }

        /// <summary>
        /// Gets whether the command is to be executed on an event or not.
        /// </summary>
        /// <param name="source">The ScriptSource to check.</param>
        /// <returns>True if the command is to be run on an event, false if not.</returns>
        private bool IsEventTriggered(ScriptScope scope)
        {
            return scope.ContainsVariable(ScriptVariables.IsEvent) && scope.GetVariable<bool>(ScriptVariables.IsEvent);
        }

        private List<string> GetEventsToRegister(ScriptScope scope)
        {
            if (scope.ContainsVariable(ScriptVariables.EventsToRegister))
            {
                var events = scope.GetVariable(ScriptVariables.EventsToRegister);
                var returnList = new List<string>();
                foreach (var ev in events)
                {
                    returnList.Add(ev);
                }
                return returnList;
            }

            return null;
        }

        /// <summary>
        /// Gets the parameters from a command script source.
        /// </summary>
        /// <param name="source">The ScriptSource to get the parameters for.</param>
        /// <returns>A list of parameter names for the script.</returns>
        private List<CommandParameter> GetCommandParameters(ScriptScope scope)
        {
            if (scope.ContainsVariable(ScriptVariables.Parameters))
            {
                var parameters = scope.GetVariable(ScriptVariables.Parameters);
                var returnList = new List<CommandParameter>();
                foreach (var param in parameters)
                {
                    returnList.Add(new CommandParameter { Key = param, IsRequired = parameters[param] });
                }

                return returnList;
            }

            return null;
        }

        /// <summary>
        /// Get the point cost from a command script source.
        /// </summary>
        /// <param name="source">The ScriptSource to get the parameters for.</param>
        /// <returns>The point cost for the command the script creates.</returns>
        private int GetCommandCost(ScriptScope scope)
        {
            if (scope.ContainsVariable(ScriptVariables.PointCost))
            {
                var cost = scope.GetVariable(ScriptVariables.PointCost);
                return cost;
            }

            return 0;
        }

        public void ExecuteEventCommand(Command command, EventScriptInfo info)
        {
            var scope = this.CreateScope();
            command.Source.Execute(scope);
            var execute = scope.GetVariable<Func<dynamic, dynamic>>(ScriptFunctions.Execute);
            execute(info);
        }

        /// <summary>
        /// Execute a command with parameters.
        /// </summary>
        /// <param name="command">The command to exeute.</param>
        /// <param name="username">The user calling the command.</param>
        /// <param name="chatParameters">Any parameters for the command.</param>
        /// <returns>A CommandResponse object with the response of the command execution.</returns>
        public void ExecuteCommand(Command command, string username, List<string> chatParameters)
        {
            var scope = this.CreateScope();
            var paramsObject = this.CreateParamObject(command, chatParameters);
            command.Source.Execute(scope);
            var execute = scope.GetVariable<Func<string, dynamic, string>>(ScriptFunctions.Execute);
            execute(username, paramsObject);
        }

        /// <summary>
        /// Eecute a command with no parameters.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="username">The user calling the command.</param>
        /// <returns>A commandResponse object with the response of the command execution.</returns>
        public void ExecuteCommand(Command command, string username)
        {
            var scope = this.CreateScope();
            command.Source.Execute(scope);
            var execute = scope.GetVariable<Func<string, string>>(ScriptFunctions.Execute);
            execute(username);
        }

        /// <summary>
        /// Create a dynamic object that contains the command parameters, that is passed to the script execute function so that we can access the script can access parameters as member variables of the param object.
        /// </summary>
        /// <param name="command">The command to create the parameter object for.</param>
        /// <param name="chatParams">The parameters passed to the command from chat.</param>
        /// <returns>A dynamic object that has member variables with the command parameter names, with their values assigned to the values from chat.</returns>
        private dynamic CreateParamObject(Command command, List<string> chatParams)
        {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            if (chatParams.Count >= command.Parameters.Count)
            {
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    obj.Add(command.Parameters[i].Key, chatParams[i]);
                }
            }

            return obj;
        }
    }
}

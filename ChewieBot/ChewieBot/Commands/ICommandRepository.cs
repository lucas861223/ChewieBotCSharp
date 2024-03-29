﻿using ChewieBot.ScriptingEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Commands
{
    public interface ICommandRepository
    {
        void LoadCommands();
        void ReloadCommand(string commandName);
        void ExecuteCommand(string commandName, string username, List<string> chatParameters);
        List<Command> GetAllCommands();
        Command GetCommand(string commandName);
        string GetCommandSource(string commandName);
        void UpdateCommandSource(string commandName, string source);
    }
}

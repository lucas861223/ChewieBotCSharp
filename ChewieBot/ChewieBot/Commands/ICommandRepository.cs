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
        void ExecuteCommand(string commandName, string username, List<string> chatParameters);
        List<Command> GetAllCommands();
        Command GetCommand(string commandName);
        int GetCommandCost(string commandName);
    }
}

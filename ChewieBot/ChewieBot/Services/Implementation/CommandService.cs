using ChewieBot.Commands;
using ChewieBot.Database.Model;
using ChewieBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class CommandService : ICommandService
    {
        private ICommandRepository commandRepository;
        private IUserService userService;
        private IUserLevelService userLevelService;
        private IVIPLevelService vipLevelService;

        private bool initialized = false;

        public CommandService(ICommandRepository commandRepository, IUserService userService, IUserLevelService userLevelService, IVIPLevelService vipLevelService)
        {
            this.commandRepository = commandRepository;
            this.userService = userService;
            this.userLevelService = userLevelService;
            this.vipLevelService = vipLevelService;
        }

        public void Initialize()
        {
            if (!this.initialized)
            {
                this.commandRepository.LoadCommands();
                this.initialized = true;
            }
        }

        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="commandName">The name for the command. This is the string immediately after the command identifier - for the command !addpoints user 10, the commandName is addpoints</param>
        /// <param name="username">The user who is executing the command.</param>
        /// <param name="chatParameters">The parameters passed to the command from the message. This is a list of space separated words from the same message as the command 
        /// - for the command !addpoints user 10, the chat parameters are 'user' and '10'.</param>
        /// <returns>A CommandResponse for the executed command, containing the status and a message if the command returns a message.</returns>
        public void ExecuteCommand(string commandName, string username, List<string> chatParameters = null) 
        {
            if (!this.initialized)
            {
                this.Initialize();
            }

            var user = this.userService.GetUser(username);
            
            var userLevel = user.UserLevel.Rank;
            var vipLevel = user.VIPLevel.Rank;
            var userPoints = user.Points;
            var commandCost = this.commandRepository.GetCommand(commandName).PointCost;
            if (!this.UserHasPermission(commandName, user))
            {
                throw new CommandException($"{username} doesn't have permission for the !{commandName} command.", commandName, true);
            }
            else if (userPoints < commandCost)
            {
                throw new CommandException($"{username} doesn't have enough points for the !{commandName} command.", commandName, true);                
            }
            else
            {
                this.commandRepository.ExecuteCommand(commandName, username, chatParameters);
                this.userService.RemovePointsForUser(username, commandCost);
            }
        }

        private bool UserHasPermission(string commandName, User user)
        {
            var command = this.commandRepository.GetCommand(commandName);
            var permission = true;

            // Only a single permission can be used. User level takes priority over vip levels.s
            if (command.RequiredUserLevelRank != null)
            {
                permission = user.UserLevel.Rank != command.RequiredUserLevelRank;
            }
            else if (command.MinimumUserLevelRank != null)
            {
                permission = user.UserLevel.Rank >= command.MinimumUserLevelRank;
            }
            else if (command.RequiredVIPLevelRank != null)
            {
                permission = user.VIPLevel.Rank != command.RequiredVIPLevelRank;
            }
            else if (command.MinimumVIPLevelRank != null)
            {
                permission = user.VIPLevel.Rank >= command.MinimumVIPLevelRank;
            }


            // Mods/Broadcaster automatically get permission for all levels below them.
            var streamer = this.userLevelService.Get("Broadcaster");
            if ((command.MinimumUserLevelRank == null || command.MinimumUserLevelRank <= streamer.Rank) && (command.RequiredUserLevelRank == null || command.RequiredUserLevelRank <= streamer.Rank))
            {
                permission = user.UserLevel.Rank >= streamer.Rank;
            }

            var bot = this.userLevelService.Get("Bot");
            if ((command.MinimumUserLevelRank == null || command.MinimumUserLevelRank <= bot.Rank) && (command.RequiredUserLevelRank == null || command.RequiredUserLevelRank <= bot.Rank))
            {
                permission = user.UserLevel.Rank >= bot.Rank;
            }

            var seniorMod = this.userLevelService.Get("SeniorModerator");
            if ((command.MinimumUserLevelRank == null || command.MinimumUserLevelRank <= seniorMod.Rank) && (command.RequiredUserLevelRank == null || command.RequiredUserLevelRank <= seniorMod.Rank))
            {
                permission = user.UserLevel.Rank >= seniorMod.Rank;
            }

            var mod = this.userLevelService.Get("Moderator");
            if ((command.MinimumUserLevelRank == null ||command.MinimumUserLevelRank <= mod.Rank) && (command.RequiredUserLevelRank == null || command.RequiredUserLevelRank <= mod.Rank))
            {
                permission = user.UserLevel.Rank >= mod.Rank;
            }

            return permission;
        }
    }
}

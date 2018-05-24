using ChewieBot.Database.Model;
using ChewieBot.Enum;
using ChewieBot.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace ChewieBot.Services.Implementation
{
    public class TwitchService : ITwitchService
    {
        private ITwitchClient client;
        private ITwitchApi api;
        private IUserService userService;
        private ICommandService commandService;

        private List<User> currentUserList;

        public TwitchService(ITwitchClient client, ITwitchApi api, IUserService userService, ICommandService commandService)
        {
            this.client = client;
            this.api = api;
            this.userService = userService;
            this.commandService = commandService;
            this.currentUserList = new List<User>();
        }

        private void SetupEventHandlers()
        {
            this.client.OnUserJoined += OnUserJoined;
            this.client.OnUserLeft += OnUserLeft;
            this.client.OnChatCommandReceived += OnChatCommandReceived;
        }

        private void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            var user = this.userService.GetUser(e.Username);
            if (user == null)
            {
                user = new User() { Username = e.Username };
                this.userService.SetUser(user);
            }

            if (!this.currentUserList.Any(x => x.Id == user.Id))
            {
                this.currentUserList.Add(user);
            }

            this.client.SendMessage($"{user.Username} joined!");
        }

        private void OnUserLeft(object sender, OnUserLeftArgs e)
        {
            var user = this.userService.GetUser(e.Username);
            if (user != null)
            {
                if (this.currentUserList.Any(x => x.Id == user.Id))
                {
                    this.currentUserList.Remove(user);
                }
            }

            this.client.SendMessage($"{user.Username} left!");
        }

        private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            var commandResponse = this.commandService.ExecuteCommand(e.Command.CommandText, e.Command.ChatMessage.Username, e.Command.ArgumentsAsList);
            if (commandResponse.ResponseType == "Message")
            {
                this.client.SendMessage(commandResponse.ToString());
            }
        }

        public void Initialize()
        {
            this.client.Initialize();
            this.SetupEventHandlers();
        }

        public void Connect()
        {
            this.client.Connect();
            this.currentUserList = this.api.GetChatters();
        }

        public void Disconnect()
        {
            this.client.Disconnect();
        }

        public IEnumerable<User> GetCurrentUsers()
        {
            return this.currentUserList;
        }
    }
}

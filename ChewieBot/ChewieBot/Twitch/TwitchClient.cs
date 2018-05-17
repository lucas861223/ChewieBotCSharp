using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using ChewieBot.Config;
using TwitchLib.Client.Events;
using ChewieBot.Services;
using ChewieBot.Database.Model;

namespace ChewieBot.Twitch
{
    public class TwitchClient
    {
        private TwitchLib.Client.TwitchClient client;
        private IUserService userService;

        public TwitchClient()
        {
            this.userService = new UserService();
        }

        public void Start()
        {
            var credentials = new ConnectionCredentials(AppConfig.TwitchUsername, AppConfig.TwitchOAuth);

            client = new TwitchLib.Client.TwitchClient();
            client.Initialize(credentials, AppConfig.TwitchChannel);

            client.OnMessageReceived += OnMessageReceived;
            client.OnJoinedChannel += OnJoinedChannel;
            client.OnUserJoined += OnUserJoined;

            client.Connect();
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            client.SendMessage(e.ChatMessage.Channel, $"Message!! -- {e.ChatMessage.Message}");

            if (userService.GetUser(e.ChatMessage.Username) == null)
            {
                var newUser = new User();
                newUser.Username = e.ChatMessage.Username;
                userService.SetUser(newUser);
            }
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            client.SendMessage(e.Channel, $"Joined channel!!");
        }

        private void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            if (userService.GetUser(e.Username) == null)
            {
                var newUser = new User();
                newUser.Username = e.Username;
                userService.SetUser(newUser);
            }
        }
    }
}

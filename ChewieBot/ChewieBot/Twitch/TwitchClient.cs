using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using ChewieBot.Config;
using TwitchLib.Client.Events;

namespace ChewieBot.Twitch
{
    public class TwitchClient
    {
        private TwitchLib.Client.TwitchClient client;

        public TwitchClient()
        {

        }

        public void Start()
        {
            var credentials = new ConnectionCredentials(AppConfig.TwitchUsername, AppConfig.TwitchOAuth);

            client = new TwitchLib.Client.TwitchClient();
            client.Initialize(credentials, AppConfig.TwitchChannel);

            client.OnMessageReceived += OnMessageReceived;
            client.OnJoinedChannel += OnJoinedChannel;

            client.Connect();
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            client.SendMessage(e.ChatMessage.Channel, $"Message!! -- {e.ChatMessage.Message}");
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            client.SendMessage(e.Channel, $"Joined channel!!");
        }
    }
}

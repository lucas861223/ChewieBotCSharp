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
using TwitchLib.Client.Services;

namespace ChewieBot.Twitch
{
    /// <summary>
    /// Sets up and Wraps the TwitchLib TwitchClient class.
    /// </summary>
    public class TwitchClient : ITwitchClient
    {
        private TwitchLib.Client.TwitchClient client;
        private IUserService userService;

        public TwitchClient(IUserService userService)
        {
            this.userService = userService;
        }

        public void Initialize()
        {
            var credentials = new ConnectionCredentials(AppConfig.TwitchUsername, AppConfig.TwitchOAuth);

            // Setup client.
            this.client = new TwitchLib.Client.TwitchClient();
            this.client.Initialize(credentials, AppConfig.TwitchChannel);
            this.client.DisableAutoPong = false;            
            this.client.AddChatCommandIdentifier('!');

            // Setup event handling.
            this.client.OnMessageReceived += OnMessageReceived;
            this.client.OnJoinedChannel += OnJoinedChannel;
            this.client.OnUserJoined += ClientUserJoined;
            this.client.OnUserLeft += ClientUserLeft;
        }

        public void Connect()
        {
            if (this.client != null && !this.client.IsConnected)
            {
                this.client.Connect();
            }
        }

        public void Disconnect()
        {
            if (this.client != null && this.client.IsConnected)
            {
                this.client.Disconnect();
            }
        }

        public void SendMessage(string message)
        {
            this.client.SendMessage(AppConfig.TwitchChannel, message);
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
        }

        // Exposing TwitchClient events

        private void ClientUserJoined(object sender, OnUserJoinedArgs e)
        {
            this.OnUserJoined?.Invoke(this, e);
        }

        private void ClientUserLeft(object sender, OnUserLeftArgs e)
        {
            this.OnUserLeft?.Invoke(this, e);
        }

        public event EventHandler<OnUserJoinedArgs> OnUserJoined;
        public event EventHandler<OnUserLeftArgs> OnUserLeft;
    }
}

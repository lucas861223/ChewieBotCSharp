using ChewieBot.Config;
using ChewieBot.Database.Model;
using ChewieBot.Enums;
using ChewieBot.Exceptions;
using ChewieBot.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace ChewieBot.Services.Implementation
{
    public class TwitchService : ITwitchService
    {
        private TwitchClient client;
        private IUserService userService;
        private ICommandService commandService;

        private List<User> currentUserList;

        public bool IsConnected { get { return this.client.IsConnected; } }

        /// <summary>
        /// Service for interacting with the Twitch Client.
        /// </summary>
        /// <param name="userService">Service for interacting with users.</param>
        /// <param name="commandService">Service for interacting with commands.</param>
        public TwitchService(IUserService userService, ICommandService commandService)
        {
            this.userService = userService;
            this.commandService = commandService;
            this.currentUserList = new List<User>();
        }

        /// <summary>
        /// Set the configuration options for the twitch client and initialize it.
        /// </summary>
        public void InitializeClient()
        {
            var credentials = new ConnectionCredentials(AppConfig.TwitchUsername, AppConfig.TwitchOAuth);

            // Setup client.
            this.client = new TwitchClient();
            this.client.Initialize(credentials, AppConfig.TwitchChannel);
            this.client.DisableAutoPong = false;
            this.client.AddChatCommandIdentifier('!');
            this.client.AddWhisperCommandIdentifier('!');

            this.SetupEventHandlers();
        }

        /// <summary>
        /// Attach event handlers to the twitch client.
        /// </summary>
        private void SetupEventHandlers()
        {
            this.client.OnMessageReceived += OnMessageReceived;
            this.client.OnJoinedChannel += OnJoinedChannel;
            this.client.OnUserJoined += OnUserJoined;
            this.client.OnUserLeft += OnUserLeft;
            this.client.OnChatCommandReceived += OnChatCommandReceived;
            this.client.OnWhisperCommandReceived += OnWhisperCommandReceived;
            this.client.OnExistingUsersDetected += OnExistingUsersDetected;
        }

        /// <summary>
        /// Fired when notified of existing users from the Twitch Client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing the channel and list of users.</param>
        private void OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e)
        {
            var userList = this.userService.GetUsers(e.Users);
            var newUsernames = e.Users.Where(x => !userList.Any(y => y.Username == x));
            var newUserList = new List<User>();
            foreach (var newUser in newUsernames)
            {
                var user = new User { Username = newUser, Points = 0 };
                newUserList.Add(user);
            }
            this.userService.SetUsers(newUserList);
            this.currentUserList = userList;
        }

        /// <summary>
        /// Fired when joining a channel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing the name of the bot and channel that was joined.</param>
        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
        }

        /// <summary>
        /// Fired when a user joins the channel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing the name of the user and channel that was joined.</param>
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

            this.SendMessage($"{user.Username} joined!");
        }

        /// <summary>
        /// Fired when a user leaves a channel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing the name of the user and channel that was left.</param>
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
        }

        /// <summary>
        /// Fired when a user sends a message to chat that starts with the set command identifier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing details about the command and message sent.</param>
        private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            try
            {
                this.commandService.ExecuteCommand(e.Command.CommandText, e.Command.ChatMessage.Username, e.Command.ArgumentsAsList);
            }
            catch (CommandPointsException cpex)
            {
                this.SendMessage(cpex.Message);
            }
            catch (CommandNotExistException cnex)
            {
                this.SendMessage($"{cnex.CommandName} does not exist.");
            }
        }

        /// <summary>
        /// Fired when a user sends a whisper to the bot that starts with the set whisper command identifier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing details about the whispered command and message sent.</param>
        private void OnWhisperCommandReceived(object sender, OnWhisperCommandReceivedArgs e)
        {
            this.commandService.ExecuteCommand(e.Command.CommandText, e.Command.WhisperMessage.Username, e.Command.ArgumentsAsList);
        }

        /// <summary>
        /// Fired when a user sends a message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing details about the message.</param>
        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
        }

        /// <summary>
        /// Connect the client to the configured Twitch channels.
        /// </summary>
        public void Connect()
        {
            if (!this.client.IsConnected)
            {
                this.client.Connect();
            }
        }

        /// <summary>
        /// Disconnect the client.
        /// </summary>
        public void Disconnect()
        {
            if (this.client.IsConnected)
            {
                this.client.Disconnect();
            }
        }

        /// <summary>
        /// Get the list of currently connected users.
        /// </summary>
        /// <returns>A list if currently connected users.</returns>
        public IEnumerable<User> GetCurrentUsers()
        {
            return this.currentUserList;
        }

        /// <summary>
        /// Send a message to the configured channel.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(string message)
        {
            this.client.SendMessage(AppConfig.TwitchChannel, message);
        }
    }
}

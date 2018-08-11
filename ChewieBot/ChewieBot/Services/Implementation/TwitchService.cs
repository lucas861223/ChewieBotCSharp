using ChewieBot.Config;
using ChewieBot.Constants;
using ChewieBot.Constants.SettingsConstants;
using ChewieBot.Database.Model;
using ChewieBot.Enums;
using ChewieBot.Events;
using ChewieBot.Events.TwitchPubSub;
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
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace ChewieBot.Services.Implementation
{
    public class TwitchService : ITwitchService
    {
        private TwitchClient client;
        private TwitchPubSub pubsubClient;
        private IUserService userService;
        private IUserLevelService userLevelService;
        private ICommandService commandService;

        public bool IsConnected { get { return this.client.IsConnected; } }
        public bool IsInitialized { get; private set; }

        public event EventHandler<OnConnectedArgs> OnConnectedEvent;
        public event EventHandler<OnDisconnectedArgs> OnDisconnectedEvent;

        // All of these events are wrappers around the TwitchPubSub events/args. 
        // We need to duplicate them in this project so that we can expose them to the python engine
        // without having to add a reference to TwitchLib.
        public event EventHandler<StreamUpArgs> OnStreamUpEvent;
        public event EventHandler<StreamDownArgs> OnStreamDownEvent;
        public event EventHandler<BitsReceivedArgs> OnBitsReceivedEvent;
        public event EventHandler<ChannelSubscriptionArgs> OnChannelSubscriptionEvent;
        public event EventHandler<HostArgs> OnHostEvent;

        /// <summary>
        /// Service for interacting with the Twitch Client.
        /// </summary>
        /// <param name="userService">Service for interacting with users.</param>
        /// <param name="commandService">Service for interacting with commands.</param>
        public TwitchService(IUserService userService, ICommandService commandService)
        {
            this.userService = userService;
            this.commandService = commandService;
        }

        /// <summary>
        /// Set the configuration options for the twitch client and initialize it.
        /// </summary>
        public void InitializeClient()
        {
            if (!this.IsInitialized)
            {
                this.commandService.Initialize();
                var credentials = new ConnectionCredentials(AppConfig.TwitchUsername, AppConfig.TwitchOAuth);

                // Setup client.
                this.client = new TwitchClient();
                this.client.Initialize(credentials, AppConfig.TwitchChannel);
                this.client.DisableAutoPong = false;
                this.client.AddChatCommandIdentifier('!');
                this.client.AddWhisperCommandIdentifier('!');

                this.pubsubClient = new TwitchPubSub();
                this.pubsubClient.ListenToVideoPlayback(AppConfig.TwitchChannel);

                this.SetupEventHandlers();

                this.IsInitialized = true;
            }
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
            this.client.OnConnected += OnConnected;
            this.client.OnDisconnected += OnDisconnected;

            this.pubsubClient.OnPubSubServiceConnected += OnPubSubServiceConnected;
            this.pubsubClient.OnStreamUp += OnStreamUp;
            this.pubsubClient.OnStreamDown += OnStreamDown;
            this.pubsubClient.OnBitsReceived += OnBitsReceived;
            this.pubsubClient.OnChannelSubscription += OnChannelSubscription;
            this.pubsubClient.OnHost += OnHost;
            this.pubsubClient.OnListenResponse += OnListenResponse;
        }

        private void OnListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
            {
                Console.WriteLine($"Failed to listen - {e.Response}");
            }
        }

        private void OnHost(object sender, OnHostArgs e)
        {
            this.OnHostEvent?.Invoke(sender, new HostArgs(e) { TriggeredByEvent = AppConstants.TwitchEvents.OnHost });
        }

        private void OnChannelSubscription(object sender, OnChannelSubscriptionArgs e)
        {
            this.OnChannelSubscriptionEvent?.Invoke(sender, new ChannelSubscriptionArgs(e) { TriggeredByEvent = AppConstants.TwitchEvents.OnChannelSubscription });
        }

        private void OnBitsReceived(object sender, OnBitsReceivedArgs e)
        {
            this.OnBitsReceivedEvent?.Invoke(sender, new BitsReceivedArgs(e) { TriggeredByEvent = AppConstants.TwitchEvents.OnBitsReceived });
        }

        private void OnStreamDown(object sender, OnStreamDownArgs e)
        {
            this.OnStreamDownEvent?.Invoke(sender, new StreamDownArgs(e) { TriggeredByEvent = AppConstants.TwitchEvents.OnStreamDown });
        }

        private void OnStreamUp(object sender, OnStreamUpArgs e)
        {
            this.OnStreamUpEvent?.Invoke(sender, new StreamUpArgs(e) { TriggeredByEvent = AppConstants.TwitchEvents.OnStreamUp });
        }

        private void OnPubSubServiceConnected(object sender, EventArgs e)
        {
            this.pubsubClient.SendTopics();
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            this.OnConnectedEvent?.Invoke(sender, e);
        }

        private void OnDisconnected(object sender, OnDisconnectedArgs e)
        {
            this.OnDisconnectedEvent?.Invoke(sender, e);
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
        }

        /// <summary>
        /// Fired when joining a channel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing the name of the bot and channel that was joined.</param>
        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            // Bot joined.
            this.OnStreamUpEvent?.Invoke(this, new StreamUpArgs(new OnStreamUpArgs { PlayDelay = 10, ServerTime = DateTime.Now.ToString() }) { TriggeredByEvent = AppConstants.TwitchEvents.OnStreamUp });
        }

        /// <summary>
        /// Fired when a user joins the channel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Args containing the name of the user and channel that was joined.</param>
        private void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            // If the viewer talked in chat before the JOIN command was received, they'll already be added as a user
            // so we need to skip this.
            if (!this.userService.IsUserWatching(e.Username))
            {
                var user = this.userService.GetUser(e.Username);
                if (user == null)
                {
                    user = this.userService.AddNewUser(e.Username, UserLevelSettings.ViewerUserLevelName);
                }
                this.userService.UserJoined(user);

                this.SendMessage($"{user.Username} joined!");

            }
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
                this.userService.UserLeft(user);
                this.SendMessage($"{user.Username} left!");
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
            catch (CommandNotExistException cnex)
            {
                this.SendMessage($"{cnex.CommandName} does not exist.");
            }
            catch (CommandException cex)
            {
                if (cex.ShouldSendToClient)
                {
                    this.SendMessage(cex.Message);
                }
            }
            catch (Exception ex)
            {
                // python script failed for some reason.
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
            // To handle if a viewer talks before the JOIN message has been received.
            if (!this.userService.IsUserWatching(e.ChatMessage.Username))
            {
                var user = this.userService.GetUser(e.ChatMessage.Username);
                if (user == null)
                {
                    user = this.userService.AddNewUser(e.ChatMessage.Username, UserLevelSettings.ViewerUserLevelName);
                }
                this.userService.UserJoined(user);
            }
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
        /// Send a message to the configured channel.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(string message)
        {
            this.client.SendMessage(AppConfig.TwitchChannel, message);
        }
    }
}

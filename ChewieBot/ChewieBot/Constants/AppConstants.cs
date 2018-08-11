using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Constants
{
    public class AppConstants
    {
        public class ConnectButton
        {
            public const string Connect = "connect";
            public const string Disconnect = "disconnect";
        }

        public class ConnectStatus
        {
            public const string Connected = "status: connected";
            public const string NotConnected = "status: not connected";
            public const string ConnectedColourHex = "#0cd64f";
            public const string NotConnectedColourHex = "#d31515";
            public const string Connecting = "status: connecting...";
        }

        public class Views
        {
            public const string SongQueue = "Song Queue";
            public const string Quotes = "Quotes";
            public const string CommandList = "Command List";
            public const string UserLevels = "User Levels";
        }

        public class PopoutMusicPlayer
        {
            public const string Text = "Popout Music Player";
        }

        public class ServiceAssmebly
        {
            public const string Path = "ChewieBot.Services.Implementation";
            public const string InterfacePath = "ChewieBot.Services";
        }

        public class Events
        {
            public const string OnStreamUp = "OnStreamUpEvent";
            public const string OnStreamDown = "OnStreamDownEvent";
            public const string OnHost = "OnHostEvent";
            public const string OnBitsReceived = "OnBitsReceivedEvent";
            public const string OnChannelSubscription = "OnChannelSubscriptionEvent";

            public const string OnEventStarted = "OnEventStartedEvent";
            public const string OnEventEnded = "OnEventEndedEvent";
        }
    }
}

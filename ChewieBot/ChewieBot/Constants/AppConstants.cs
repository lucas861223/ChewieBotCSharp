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
        }

        public class PopoutMusicPlayer
        {
            public const string Text = "Popout Music Player";
        }
    }
}

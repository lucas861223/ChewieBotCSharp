using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace ChewieBot.Twitch
{
    public interface ITwitchClient
    {
        void Initialize();
        void Connect();
        void Disconnect();
        void SendMessage(string message);

        event EventHandler<OnUserJoinedArgs> OnUserJoined;
        event EventHandler<OnUserLeftArgs> OnUserLeft;
    }
}

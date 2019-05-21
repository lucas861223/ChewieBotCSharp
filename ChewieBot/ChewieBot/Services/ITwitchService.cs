using ChewieBot.Database.Model;
using ChewieBot.Events.TwitchPubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Events;

namespace ChewieBot.Services
{
    public interface ITwitchService
    {
        void Connect();
        void Disconnect();
        void SendMessage(string message);
        bool IsConnected { get; }
        bool IsInitialized { get; }
        void InitializeClient();

        event EventHandler<OnConnectedArgs> OnConnectedEvent;
        event EventHandler<OnDisconnectedEventArgs> OnDisconnectedEvent;
        event EventHandler<StreamUpArgs> OnStreamUpEvent;
        event EventHandler<StreamDownArgs> OnStreamDownEvent;
        event EventHandler<ChannelSubscriptionArgs> OnChannelSubscriptionEvent;
        event EventHandler<BitsReceivedArgs> OnBitsReceivedEvent;
        event EventHandler<HostArgs> OnHostEvent;
    }
}

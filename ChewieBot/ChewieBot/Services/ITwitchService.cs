using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

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
        event EventHandler<OnDisconnectedArgs> OnDisconnectedEvent;
    }
}

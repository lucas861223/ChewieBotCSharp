using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub.Events;

namespace ChewieBot.Events.TwitchPubSub
{
    public class BitsReceivedArgs : TwitchEventArgs
    {
        public int BitsUsed { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChatMessage { get; set; }
        public string Context { get; set; }
        public string Time { get; set; }
        public int TotalBitsUsed { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }

        public BitsReceivedArgs(OnBitsReceivedArgs args)
        {
            this.BitsUsed = args.BitsUsed;
            this.ChannelId = args.ChannelId;
            this.ChannelName = args.ChannelName;
            this.ChatMessage = args.ChatMessage;
            this.Context = args.Context;
            this.Time = args.Time;
            this.TotalBitsUsed = args.TotalBitsUsed;
            this.UserId = args.UserId;
            this.Username = args.Username;
        }
    }
}

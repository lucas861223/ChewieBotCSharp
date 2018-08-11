using ChewieBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub.Events;

namespace ChewieBot.Events.TwitchPubSub
{
    public class ChannelSubscriptionArgs
    {
        public ChannelSubscription Subscription { get; set; }

        public ChannelSubscriptionArgs(OnChannelSubscriptionArgs args)
        {
            this.Subscription = new ChannelSubscription(args.Subscription);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub.Events;

namespace ChewieBot.Events.TwitchPubSub
{
    public class HostArgs
    {
        public string HostedChannel { get; set; }
        public string Moderator { get; set; }

        public HostArgs(OnHostArgs args)
        {
            this.HostedChannel = args.HostedChannel;
            this.Moderator = args.Moderator;
        }
    }
}

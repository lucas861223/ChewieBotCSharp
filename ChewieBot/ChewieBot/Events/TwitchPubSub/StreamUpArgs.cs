using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub.Events;

namespace ChewieBot.Events.TwitchPubSub
{
    public class StreamUpArgs
    {
        public int PlayDelay { get; set; }
        public string ServerTime { get; set; }

        public StreamUpArgs(OnStreamUpArgs args)
        {
            this.PlayDelay = args.PlayDelay;
            this.ServerTime = args.ServerTime;
        }
    }
}

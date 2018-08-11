using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub.Events;

namespace ChewieBot.Events.TwitchPubSub
{
    public class StreamDownArgs
    {
        public string ServerTime { get; set; }

        public StreamDownArgs(OnStreamDownArgs args)
        {
            this.ServerTime = args.ServerTime;
        }
    }
}

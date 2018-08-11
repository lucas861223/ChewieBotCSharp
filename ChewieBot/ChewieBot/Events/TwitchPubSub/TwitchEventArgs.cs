using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Events.TwitchPubSub
{
    public abstract class TwitchEventArgs
    {
        public string TriggeredByEvent { get; set; }
    }
}

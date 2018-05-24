using ChewieBot.Database.Model;
using ChewieBot.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Scripting.Events
{
    public class EventStartedEventArgs
    {
        public ChatEvent ChatEvent { get; set; }
    }
}

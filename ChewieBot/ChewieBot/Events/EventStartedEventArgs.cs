using ChewieBot.Database.Model;
using ChewieBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Events
{
    public class EventStartedEventArgs : BaseEvent
    {
        public int EventId { get; set; }
    }
}

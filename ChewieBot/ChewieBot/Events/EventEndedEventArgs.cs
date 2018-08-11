using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Events
{
    public class ChatEventEndedEventArgs : BaseEvent
    {
        public ChatEvent ChatEvent { get; set; }
        public List<EventWinner> EventWinners { get; set; }
    }
}

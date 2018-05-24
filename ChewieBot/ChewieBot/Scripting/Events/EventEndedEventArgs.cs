﻿using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Scripting.Events
{
    public class EventEndedEventArgs
    {
        public ChatEvent ChatEvent { get; set; }
        public List<EventWinner> EventWinners { get; set; }
    }
}

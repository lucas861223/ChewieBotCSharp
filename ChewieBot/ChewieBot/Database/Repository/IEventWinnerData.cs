﻿using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IEventWinnerData
    {
        EventWinner Set(EventWinner eventWinner);
        IEnumerable<EventWinner> Get(ChatEvent chatEvent);
        IEnumerable<EventWinner> GetAllForUser(User user);
    }
}

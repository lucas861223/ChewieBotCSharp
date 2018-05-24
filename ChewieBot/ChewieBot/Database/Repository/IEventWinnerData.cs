using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IEventWinnerData
    {
        EventWinner SetEventWinner(EventWinner eventWinner);
        IEnumerable<EventWinner> GetEventWinners(ChatEvent chatEvent);
        IEnumerable<EventWinner> GetEventWinsForUser(User user);
    }
}

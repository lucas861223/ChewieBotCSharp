using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IEventWinnerService
    {
        IEnumerable<EventWinner> GetEventWinners(int eventId);
        IEnumerable<EventWinner> GetEventWinners(ChatEvent chatEvent);
        EventWinner SetEventWinner(EventWinner eventWinner);
        IEnumerable<EventWinner> GetEventWinsForUser(User user);
        IEnumerable<EventWinner> GetEventWinsForUser(string username);
    }
}

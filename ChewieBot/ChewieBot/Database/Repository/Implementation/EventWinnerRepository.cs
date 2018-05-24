using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;

namespace ChewieBot.Database.Repository.Implementation
{
    public class EventWinnerRepository : IEventWinnerData
    {
        public IEnumerable<EventWinner> GetEventWinners(ChatEvent chatEvent)
        {
            using (var context = new DatabaseContext())
            {
                return context.EventWinners.Where(x => x.Event == chatEvent);
            }
        }

        public IEnumerable<EventWinner> GetEventWinsForUser(User user)
        {
            using (var context = new DatabaseContext())
            {
                return context.EventWinners.Where(x => x.User == user);
            }
        }

        public EventWinner SetEventWinner(EventWinner eventWinner)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.EventWinners.Find(eventWinner.Id);
                if (record == null)
                {
                    record = eventWinner;
                    context.EventWinners.Add(record);
                }
                else
                {
                    context.Entry(record).CurrentValues.SetValues(eventWinner);
                }
                return record;
            }
        }
    }
}

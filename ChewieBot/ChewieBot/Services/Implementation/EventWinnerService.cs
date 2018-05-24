using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;

namespace ChewieBot.Services.Implementation
{
    public class EventWinnerService : IEventWinnerService
    {
        private IEventWinnerData eventWinnerData;
        private IChatEventData chatEventData;
        private IUserData userData;

        public EventWinnerService(IEventWinnerData eventWinnerData, IChatEventData chatEventData, IUserData userData)
        {
            this.eventWinnerData = eventWinnerData;
            this.chatEventData = chatEventData;
            this.userData = userData;
        }

        public IEnumerable<EventWinner> GetEventWinners(int eventId)
        {
            var chatEvent = this.chatEventData.GetChatEvent(eventId);
            return this.GetEventWinners(chatEvent);
        }

        public IEnumerable<EventWinner> GetEventWinners(ChatEvent chatEvent)
        {
            if (chatEvent != null)
            {
                return this.eventWinnerData.GetEventWinners(chatEvent);
            }
            return null;
        }

        public IEnumerable<EventWinner> GetEventWinsForUser(User user)
        {
            if (user != null)
            {
                return this.eventWinnerData.GetEventWinsForUser(user);
            }
            return null;
        }

        public IEnumerable<EventWinner> GetEventWinsForUser(string username)
        {
            var user = this.userData.GetUser(username);
            return this.GetEventWinsForUser(user);
        }

        public EventWinner SetEventWinner(EventWinner eventWinner)
        {
            return this.eventWinnerData.SetEventWinner(eventWinner);
        }
    }
}

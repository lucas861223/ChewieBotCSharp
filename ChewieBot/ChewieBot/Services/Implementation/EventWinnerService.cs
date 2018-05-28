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

        /// <summary>
        /// Get the list of EventWinners for an event.
        /// </summary>
        /// <param name="eventId">The event id to get the winners for.</param>
        /// <returns>The list of EventWinners for the event</returns>
        public IEnumerable<EventWinner> GetEventWinners(int eventId)
        {
            var chatEvent = this.chatEventData.GetChatEvent(eventId);
            return this.GetEventWinners(chatEvent);
        }

        /// <summary>
        /// Get the list of EventWinners for an event.
        /// </summary>
        /// <param name="chatEvent">The event to get the winners for.</param>
        /// <returns>The list of EventWinners for the event.</returns>
        public IEnumerable<EventWinner> GetEventWinners(ChatEvent chatEvent)
        {
            if (chatEvent != null)
            {
                return this.eventWinnerData.GetEventWinners(chatEvent);
            }
            return null;
        }

        /// <summary>
        /// Get the list of EventWinners for a user.
        /// </summary>
        /// <param name="user">The user to get EventWinners for.</param>
        /// <returns>The list of all EventWinners for the user.</returns>
        public IEnumerable<EventWinner> GetEventWinsForUser(User user)
        {
            if (user != null)
            {
                return this.eventWinnerData.GetEventWinsForUser(user);
            }
            return null;
        }

        /// <summary>
        /// Get the list of EventWinners for a username.
        /// </summary>
        /// <param name="username">The username to get EventWinners for.</param>
        /// <returns>The list of all EventWinners for the user.</returns>
        public IEnumerable<EventWinner> GetEventWinsForUser(string username)
        {
            var user = this.userData.GetUser(username);
            return this.GetEventWinsForUser(user);
        }

        /// <summary>
        /// Add an EventWinner to the database if it doesn't exist, or update an existing EventWinner.
        /// </summary>
        /// <param name="eventWinner">The event winner to add or update.</param>
        /// <returns>The event winner that was added or updated.</returns>
        public EventWinner SetEventWinner(EventWinner eventWinner)
        {
            return this.eventWinnerData.SetEventWinner(eventWinner);
        }
    }
}

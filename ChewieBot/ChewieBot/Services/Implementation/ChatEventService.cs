using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Enums;
using ChewieBot.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ChewieBot.Services.Implementation
{
    public class ChatEventService : IChatEventService
    {
        private Dictionary<int, ChatEvent> eventList;
        private Dictionary<int, Timer> eventTimers;
        private IChatEventData chatEventData;
        private IEventWinnerData eventWinnerData;
        private IUserService userService;

        public event EventHandler<EventStartedEventArgs> OnEventStarted;
        public event EventHandler<EventEndedEventArgs> OnEventEnded;

        public ChatEventService(IChatEventData chatEventData, IEventWinnerData eventWinnerData, IUserService userService)
        {
            this.eventList = new Dictionary<int, ChatEvent>();
            this.eventTimers = new Dictionary<int, Timer>();
            this.chatEventData = chatEventData;
            this.eventWinnerData = eventWinnerData;
            this.userService = userService;
        }


        /// <summary>
        /// Add a user to an event.
        /// </summary>
        /// <param name="eventId">The event id to add the user to.</param>
        /// <param name="user">The user to add to the event.</param>
        private void AddUserToEvent(int eventId, User user)
        {
            if (this.eventList.ContainsKey(eventId) && !this.eventList[eventId].HasFinished && !this.eventList[eventId].UserList.Any(x => x.Id == user.Id))
            {
                this.eventList[eventId].UserList.Add(user);
            }
        }

        /// <summary>
        /// Create a new event.
        /// </summary>
        /// <param name="type">The type of event that is to be created.</param>
        /// <param name="duration">The duration of the event in milliseconds.</param>
        /// <returns>The ChatEvent that was created.</returns>
        public ChatEvent CreateNewEvent(EventType type, int duration)
        {
            var chatEvent = new ChatEvent() { Type = type, Duration = duration };
            chatEvent = this.chatEventData.Set(chatEvent);
            this.eventList.Add(chatEvent.EventId, chatEvent);
            return chatEvent;
        }

        /// <summary>
        /// Start an event. This starts a timer that will trigger the StopEvent() function after the configured duration has elapsed.
        /// Invokes the EventStartedEvent.
        /// </summary>
        /// <param name="eventId">The id of the event to start.</param>
        public void StartEvent(int eventId)
        {
            if (this.eventList.ContainsKey(eventId) && !this.eventList[eventId].HasStarted && !this.eventList[eventId].HasFinished)
            {
                var chatEvent = this.eventList[eventId];
                chatEvent.HasStarted = true;
                var timer = new Timer(chatEvent.Duration);                
                timer.AutoReset = false;
                timer.Elapsed += (sender, args) =>
                {
                    this.eventTimers.Remove(eventId);
                    this.StopEvent(eventId);
                };

                this.eventTimers.Add(eventId, timer);
                this.eventTimers[eventId].Start();
                this.OnEventStarted?.Invoke(this, new EventStartedEventArgs { EventId = eventId });
            }
        }

        /// <summary>
        /// Stop an event. Invokes the EventEndedEvent.
        /// </summary>
        /// <param name="eventId">The id of the event to stop.</param>
        public void StopEvent(int eventId)
        {
            if (this.eventList.ContainsKey(eventId) && this.eventList[eventId].HasStarted)
            {
                this.eventList[eventId].HasStarted = false;
                this.eventList[eventId].HasFinished = true;
                this.eventList[eventId].TimeFinished = DateTime.UtcNow;

                this.chatEventData.Set(this.eventList[eventId]);

                var winners = this.GetEventWinners(eventId);
                this.OnEventEnded?.Invoke(this, new EventEndedEventArgs { ChatEvent = this.eventList[eventId], EventWinners = winners });

                this.eventTimers.Remove(eventId);
            }
        }

        /// <summary>
        /// Get the list of winners for an event.
        /// </summary>
        /// <param name="eventId">The id of the event to get winners for.</param>
        /// <returns>The list of winners for the event.</returns>
        public List<EventWinner> GetEventWinners(int eventId)
        {
            var winnerList = new List<EventWinner>();
            if (this.eventList.ContainsKey(eventId) && this.eventList[eventId].HasFinished)
            {
                var winners = CreateEventWinners(eventId);
                for (int i = 0; i < winners.Count; i++)
                {
                    var winner = new EventWinner() { Event = this.eventList[eventId], User = winners[i], Position = i + 1 };
                    this.eventWinnerData.Set(winner);
                    winnerList.Add(winner);
                }
            }

            return winnerList;
        }

        /// <summary>
        /// Add a user to an event by username.
        /// </summary>
        /// <param name="eventId">The id of the event to add the user to.</param>
        /// <param name="username">The username of the user to add.</param>
        public void AddUser(int eventId, string username)
        {
            var user = userService.GetUser(username);
            if (user != null)
            {
                this.AddUserToEvent(eventId, user);
            }
        }

        /// <summary>
        /// Add a user to the started event of a specified event type.
        /// </summary>
        /// <param name="type">The type of event to add the user to.</param>
        /// <param name="username">The username to add to the event.</param>
        public void AddUserToCurrentEvent(EventType type, string username)
        {
            var currentEvent = this.eventList.Values.FirstOrDefault(x => x.Type == type && x.HasStarted);
            if (currentEvent != null)
            {
                this.AddUser(currentEvent.EventId, username);
            }
        }

        /// <summary>
        /// Creates event winners for an event.
        /// </summary>
        /// <param name="eventId">The id of the event to create winners for.</param>
        /// <returns>The list of winning users for an event.</returns>
        private List<User> CreateEventWinners(int eventId)
        {
            //TODO: Update this to not be a naive implementation. Currently just used for testing.
            // Should close an event without winners if there's not enough entrants.
            // Number of winners should have an option to scale with the number of entrants.
            // Number of winners should be able to be specified.
            var winners = new List<User>();
            var winningIndexList = new List<int>();
            if (this.eventList.ContainsKey(eventId) && this.eventList[eventId].HasFinished)
            {
                var chatEvent = this.eventList[eventId];
                if (chatEvent.UserList.Count >= 1)
                {
                    var count = chatEvent.UserList.Count >= 3 ? 3 : chatEvent.UserList.Count;
                    var rnd = new Random();
                    
                    for(int i = 0; i < count; i++)
                    {
                        int winnerIndex;
                        do
                        {
                            winnerIndex = rnd.Next(chatEvent.UserList.Count);
                        } while (winningIndexList.Any(x => x == winnerIndex));

                        winningIndexList.Add(winnerIndex);
                        winners.Add(chatEvent.UserList[winnerIndex]);
                    }
                }
            }

            return winners;
        }
    }
}

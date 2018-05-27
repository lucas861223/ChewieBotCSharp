using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Enum;
using ChewieBot.Scripting.Events;
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

        private void AddUserToEvent(int eventId, User user)
        {
            if (this.eventList.ContainsKey(eventId) && !this.eventList[eventId].HasFinished && !this.eventList[eventId].UserList.Any(x => x.Id == user.Id))
            {
                this.eventList[eventId].UserList.Add(user);
            }
        }

        public ChatEvent CreateNewEvent(EventType type, int duration)
        {
            var chatEvent = new ChatEvent() { Type = type, Duration = duration };
            chatEvent = this.chatEventData.SetChatEvent(chatEvent);
            this.eventList.Add(chatEvent.EventId, chatEvent);
            return chatEvent;
        }

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

        public void StopEvent(int eventId)
        {
            if (this.eventList.ContainsKey(eventId) && this.eventList[eventId].HasStarted)
            {
                this.eventList[eventId].HasStarted = false;
                this.eventList[eventId].HasFinished = true;
                this.eventList[eventId].TimeFinished = DateTime.UtcNow;

                this.chatEventData.SetChatEvent(this.eventList[eventId]);

                var winners = this.GetEventWinners(eventId);
                this.OnEventEnded?.Invoke(this, new EventEndedEventArgs { ChatEvent = this.eventList[eventId], EventWinners = winners });

                this.eventTimers.Remove(eventId);
            }
        }

        public List<EventWinner> GetEventWinners(int eventId)
        {
            var winnerList = new List<EventWinner>();
            if (this.eventList.ContainsKey(eventId) && this.eventList[eventId].HasFinished)
            {
                var winners = CreateEventWinners(eventId);
                for (int i = 0; i < winners.Count; i++)
                {
                    var winner = new EventWinner() { Event = this.eventList[eventId], User = winners[i], Position = i + 1 };
                    this.eventWinnerData.SetEventWinner(winner);
                    winnerList.Add(winner);
                }
            }

            return winnerList;
        }

        public void AddUser(int eventId, string username)
        {
            var user = userService.GetUser(username);
            if (user != null)
            {
                this.AddUserToEvent(eventId, user);
            }
        }

        public void AddUserToCurrentEvent(string username)
        {
            var currentEvent = this.eventList.Values.FirstOrDefault(x => x.HasStarted);
            if (currentEvent != null)
            {
                this.AddUser(currentEvent.EventId, username);
            }
        }

        private List<User> CreateEventWinners(int eventId)
        {
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

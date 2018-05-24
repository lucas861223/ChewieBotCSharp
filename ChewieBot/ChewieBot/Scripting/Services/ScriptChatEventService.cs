using ChewieBot.Database.Model;
using ChewieBot.Enum;
using ChewieBot.Scripting.Events;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ChewieBot.Scripting.Services
{
    public class ScriptChatEventService
    {     
        private IChatEventService chatEventService;
        public ScriptChatEventService(IChatEventService chatEventService)
        {
            this.chatEventService = chatEventService;
            this.chatEventService.OnEventStarted += EventStarted;
            this.chatEventService.OnEventEnded += EventEnded;
        }

        public event EventHandler<EventStartedEventArgs> OnEventStarted;
        public event EventHandler<EventEndedEventArgs> OnEventEnded;

        public ChatEvent AddEvent(string type, int delay, int duration)
        {
            var eventType = (EventType)System.Enum.Parse(typeof(EventType), type);
            var chatEvent = this.chatEventService.AddEvent(eventType, delay, duration);
            return chatEvent;

        }

        public void StartEvent(int eventId)
        {
            this.chatEventService.StartEvent(eventId);
        }

        public void AddUser(int eventId, string username)
        {
            this.chatEventService.AddUser(eventId, username);
        }

        private void EventStarted(object sender, EventStartedEventArgs e)
        {
            this.OnEventStarted.Invoke(sender, e);
        }

        private void EventEnded(object sender, EventEndedEventArgs e)
        {
            this.OnEventEnded.Invoke(sender, e);
        }
    }
}

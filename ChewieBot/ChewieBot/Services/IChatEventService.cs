using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.Enum;
using ChewieBot.Scripting.Events;

namespace ChewieBot.Services
{
    public interface IChatEventService
    {
        ChatEvent AddEvent(EventType type, int delay, int duration);
        void StartEvent(int eventId);
        void StopEvent(int eventId);
        void AddUser(int eventId, string username);

        event EventHandler<EventStartedEventArgs> OnEventStarted;
        event EventHandler<EventEndedEventArgs> OnEventEnded;
    }
}

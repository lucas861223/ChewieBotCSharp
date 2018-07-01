using ChewieBot.Database.Model;
using ChewieBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IChatEventData
    {
        ChatEvent Set(ChatEvent chatEvent);
        ChatEvent Get(int eventId);
        IEnumerable<ChatEvent> GetAllForType(EventType type);
        void Delete(ChatEvent chatEvent);
        void Delete(int eventId);
    }
}

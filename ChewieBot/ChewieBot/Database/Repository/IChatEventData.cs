using ChewieBot.Database.Model;
using ChewieBot.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IChatEventData
    {
        ChatEvent SetChatEvent(ChatEvent chatEvent);
        ChatEvent GetChatEvent(int eventId);
        IEnumerable<ChatEvent> GetAllChatEventsForType(EventType type);
        void DeleteChatEvent(ChatEvent chatEvent);
        void DeleteChatEvent(int eventId);
    }
}

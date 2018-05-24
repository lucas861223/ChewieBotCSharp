using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.Enum;

namespace ChewieBot.Database.Repository.Implementation
{
    public class ChatEventRepository : IChatEventData
    {
        public IEnumerable<ChatEvent> GetAllChatEventsForType(EventType type)
        {
            using (var context = new DatabaseContext())
            {
                return context.ChatEvents.Where(x => x.Type == type);
            }
        }

        public ChatEvent GetChatEvent(int eventId)
        {
            using (var context = new DatabaseContext())
            {
                return context.ChatEvents.Find(eventId);
            }
        }

        public ChatEvent SetChatEvent(ChatEvent chatEvent)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.ChatEvents.Find(chatEvent.EventId);
                if (record == null)
                {
                    record = chatEvent;
                    context.ChatEvents.Add(record);
                }
                else
                {
                    context.Entry(record).CurrentValues.SetValues(chatEvent);
                }

                context.SaveChanges();
                return record;
            }
        }
    }
}

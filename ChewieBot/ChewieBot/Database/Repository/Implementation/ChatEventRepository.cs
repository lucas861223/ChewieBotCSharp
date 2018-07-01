using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.Enums;

namespace ChewieBot.Database.Repository.Implementation
{
    public class ChatEventRepository : IChatEventData
    {
        public IEnumerable<ChatEvent> GetAllForType(EventType type)
        {
            using (var context = new DatabaseContext())
            {
                return context.ChatEvents.Where(x => x.Type == type && !x.IsDeleted);
            }
        }

        public ChatEvent Get(int eventId)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.ChatEvents.Find(eventId);
                if (!record.IsDeleted)
                {
                    return record;
                }

                return null;
            }
        }

        public ChatEvent Set(ChatEvent chatEvent)
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

        public void Delete(ChatEvent chatEvent)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.ChatEvents.Find(chatEvent.EventId);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int eventId)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.ChatEvents.Find(eventId);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }
    }
}

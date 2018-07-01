using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;

namespace ChewieBot.Database.Repository.Implementation
{
    public class BotSettingRepository : IBotSettingData
    {
        public void Delete(BotSetting setting)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.BotSettings.Find(setting.Id);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public void Delete(string name)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.BotSettings.FirstOrDefault(x => x.Name == name);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public BotSetting Get(string name)
        {
            using (var context = new DatabaseContext())
            {
                return context.BotSettings.FirstOrDefault(x => x.Name == name && !x.IsDeleted);
            }
        }

        public BotSetting Set(BotSetting setting)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.BotSettings.Find(setting.Id);
                if (record == null)
                {
                    record = setting;
                    context.BotSettings.Add(record);
                }
                else
                {
                    context.Entry(record).CurrentValues.SetValues(setting);
                }

                context.SaveChanges();
                return record;
            }
        }
    }
}

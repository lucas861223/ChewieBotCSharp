using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class BotSettingService : IBotSettingService
    {
        private IBotSettingData settingData;

        public BotSettingService(IBotSettingData settingData)
        {
            this.settingData = settingData;
        }

        public void Delete(string name)
        {
            this.settingData.Delete(name);
        }

        public void Delete(BotSetting setting)
        {
            this.settingData.Delete(setting);
        }

        public BotSetting Get(string name)
        {
            return this.settingData.Get(name);
        }

        public BotSetting Set(BotSetting setting)
        {
            return this.settingData.Set(setting);
        }
    }
}

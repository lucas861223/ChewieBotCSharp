using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IBotSettingData
    {
        BotSetting Set(BotSetting setting);
        BotSetting Get(string name);
        void Delete(BotSetting setting);
        void Delete(string name);
    }
}

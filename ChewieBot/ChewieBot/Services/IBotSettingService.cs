using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IBotSettingService
    {
        BotSetting Set(BotSetting setting);
        BotSetting Get(string name);
        void Delete(string name);
        void Delete(BotSetting setting);
    }
}

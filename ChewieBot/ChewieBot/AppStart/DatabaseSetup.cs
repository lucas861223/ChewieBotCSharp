using ChewieBot.Constants;
using ChewieBot.Constants.SettingsConstants;
using ChewieBot.Database.Model;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.AppStart
{
    public static class DatabaseSetup
    {
        private static IBotSettingService settingService = UnityConfig.Resolve<IBotSettingService>();
        private static IUserLevelService userLevelService = UnityConfig.Resolve<IUserLevelService>();

        public static void Setup()
        {
            SetupUserLevels();
            SetupBaseSettings();
        }

        private static void SetupUserLevels()
        {
            // if this is null, all the others won't exist either, so it's safe to add default values for them.
            if (userLevelService.Get(UserLevelSettings.ViewerUserLevelName) == null)
            {
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.ViewerUserLevelName, PointMultiplier = 1.0f });
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.SubscriberUserLevelName, PointMultiplier = 1.5f });
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.ModeratorUserLevelName, PointMultiplier = 1.5f });
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.BroadcasterUserLevelName, PointMultiplier = 1.5f });
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.BotUserLevelName, PointMultiplier = 0.0f });
            }
        }

        private static void SetupBaseSettings()
        {
            if (settingService.Get(BaseSettings.PointRate.Name) == null)
            {
                var bf = new BinaryFormatter();
                var ms = new MemoryStream();
                bf.Serialize(ms, 5);

                settingService.Set(new BotSetting { Name = BaseSettings.PointRate.Name, Description = BaseSettings.PointRate.Description, Value = ms.ToArray(), TypeName = typeof(double).ToString() });
            }
        }
    }
}

using ChewieBot.Constants;
using ChewieBot.Database.Model;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private static void SetupUserLevels()
        {
            // if this is null, all the others won't exist either, so it's safe to add default values for them.
            if (userLevelService.Get(SettingsConstants.UserLevels.ViewerUserLevelName) == null)
            {
                userLevelService.Set(new UserLevel { Name = SettingsConstants.UserLevels.ViewerUserLevelName, PointMultiplier = 1.0f });
                userLevelService.Set(new UserLevel { Name = SettingsConstants.UserLevels.SubscriberUserLevelName, PointMultiplier = 1.5f });
                userLevelService.Set(new UserLevel { Name = SettingsConstants.UserLevels.ModeratorUserLevelName, PointMultiplier = 1.5f });
                userLevelService.Set(new UserLevel { Name = SettingsConstants.UserLevels.BroadcasterUserLevelName, PointMultiplier = 1.5f });
                userLevelService.Set(new UserLevel { Name = SettingsConstants.UserLevels.BotUserLevelName, PointMultiplier = 0.0f });
            }
        }

        private static void SetupBaseSettings()
        {
            if (settingService.Get(SettingsConstants.BaseSettings.PointRate.Name) == null)
            {
                settingService.Set(new BotSetting { Name = SettingsConstants.BaseSettings.PointRate.Name, Description = SettingsConstants.BaseSettings.PointRate.Description, Value = "100" });
            }
        }
    }
}

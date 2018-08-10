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
        private static IModLevelService modLevelService = UnityConfig.Resolve<IModLevelService>();
        private static IVIPLevelService vipLevelService = UnityConfig.Resolve<IVIPLevelService>();

        public static void Setup()
        {
            SetupUserLevels();
            SetupModLevels();
            SetupVIPLevels();
            SetupBaseSettings();
        }

        private static void SetupUserLevels()
        {
            // if this is null, all the others won't exist either, so it's safe to add default values for them.
            if (userLevelService.Get(UserLevelSettings.ViewerUserLevelName) == null)
            {
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.ViewerUserLevelName, PointMultiplier = 1.0f, Rank = 0 });
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.SubscriberUserLevelName, PointMultiplier = 1.5f, Rank = 1 });
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.ModeratorUserLevelName, PointMultiplier = 1.5f, Rank = 2 });                
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.BotUserLevelName, PointMultiplier = 0.0f, Rank = 3 });
                userLevelService.Set(new UserLevel { Name = UserLevelSettings.BroadcasterUserLevelName, PointMultiplier = 1.5f, Rank = 4 });
            }
        }

        private static void SetupModLevels()
        {
            if (modLevelService.Get(ModLevelSettings.BotLevelName) == null)
            {
                modLevelService.Set(new ModLevel { Name = ModLevelSettings.NoModLevelName, Rank = 0 });
                modLevelService.Set(new ModLevel { Name = ModLevelSettings.ModLevelName, Rank = 1 });
                modLevelService.Set(new ModLevel { Name = ModLevelSettings.SeniorModLevelName, Rank = 2 });
                modLevelService.Set(new ModLevel { Name = ModLevelSettings.BotLevelName, Rank = 4 });
                modLevelService.Set(new ModLevel { Name = ModLevelSettings.BroadcasterLevelName, Rank = 5 });
            }
        }

        private static void SetupVIPLevels()
        {
            if (vipLevelService.Get(VIPLevelSettings.BronzeVIPLevelName) == null)
            {
                vipLevelService.Set(new VIPLevel { Name = VIPLevelSettings.BronzeVIPLevelName, Rank = 1, PointMultiplier = 1.5f });
                vipLevelService.Set(new VIPLevel { Name = VIPLevelSettings.SilverVIPLevelName, Rank = 2, PointMultiplier = 1.5f });
                vipLevelService.Set(new VIPLevel { Name = VIPLevelSettings.GoldVIPLevelName, Rank = 3, PointMultiplier = 1.5f });
                vipLevelService.Set(new VIPLevel { Name = VIPLevelSettings.NoVIPLevelName, Rank = 10, PointMultiplier = 1.0f });
            }
        }

        private static void SetupBaseSettings()
        {
            if (!settingService.Exists(BaseSettings.PointRate.Name))
            {
                settingService.Set(BaseSettings.PointRate.Name, BaseSettings.PointRate.Description, 5, typeof(double));
            }
        }
    }
}

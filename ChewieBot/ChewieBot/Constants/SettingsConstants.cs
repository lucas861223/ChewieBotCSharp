using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Constants.SettingsConstants
{
    public class UserLevelSettings
    {
        public const string ViewerUserLevelName = "Viewer";
        public const string SubscriberUserLevelName = "Subscriber";
        public const string ModeratorUserLevelName = "Moderator";
        public const string BroadcasterUserLevelName = "Broadcaster";
        public const string BotUserLevelName = "Bot";
    }

    public class ModLevelSettings
    {
        // Mod: 0
        public const string NoModLevelName = "None";
        // Mod: 1
        public const string ModLevelName = "Moderator";
        // Mod: 2
        public const string SeniorModLevelName = "Senior Moderator";
        // Mod: 4
        public const string BotLevelName = "Bot";
        // Mod: 5
        public const string BroadcasterLevelName = "Broadcaster";
    }

    public class VIPLevelSettings
    {
        // VIP: 10
        public const string NoVIPLevelName = "None";
        // VIP: 1
        public const string BronzeVIPLevelName = "Bronze";
        // VIP: 2
        public const string SilverVIPLevelName = "Silver";
        // VIP: 3
        public const string GoldVIPLevelName = "Gold";
    }

    public class BaseSettings
    {
        public class PointRate
        {
            public const string Name = "BasePointRate";
            public const string Description = "The base rate that points will accumulate per hour.";
        }
    }
}

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

    public class BaseSettings
    {
        public class PointRate
        {
            public const string Name = "BasePointRate";
            public const string Description = "The base rate that points will accumulate per hour.";
        }
    }
}

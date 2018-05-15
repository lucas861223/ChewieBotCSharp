using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Config
{
    public static class AppConfig
    {
        public static string TwitchOAuth = ConfigurationManager.AppSettings["Twitch-OAuth"];
        public static string TwitchUsername = ConfigurationManager.AppSettings["Twitch-Username"];
        public static string TwitchChannel = ConfigurationManager.AppSettings["Twitch-Channel"];
    }
}

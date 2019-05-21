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
        // Twitch Api
        public static string TwitchAccessToken = ConfigurationManager.AppSettings["Twitch-AccessToken"];
        public static string TwitchClientId = ConfigurationManager.AppSettings["Twitch-ClientId"];

        // Twitch Client
        public static string TwitchChannel = ConfigurationManager.AppSettings["Twitch-Channel"];
        public static string TwitchOAuth = ConfigurationManager.AppSettings["Twitch-OAuth"];
        public static string TwitchUsername = ConfigurationManager.AppSettings["Twitch-Username"];

        // BotSettings
        public static int SettingsCacheDuration = Int32.Parse(ConfigurationManager.AppSettings["Settings-CacheDuration"]);

        // Deepbot
        public static string DeepbotUserFile = ConfigurationManager.AppSettings["DeepbotUserFile"];

        public static string CommandsPath = ConfigurationManager.AppSettings["CommandsPath"];

        // Streamlabs
        public static string StreamlabsAuthUrl = ConfigurationManager.AppSettings["Streamlabs-AuthUrl"];
        public static string StreamlabsTokenUrl = ConfigurationManager.AppSettings["Streamlabs-TokenUrl"];
        public static string StreamlabsClientId = ConfigurationManager.AppSettings["Streamlabs-ClientId"];
        public static string StreamlabsClientSecret = ConfigurationManager.AppSettings["Streamlabs-ClientSecret"];
        public static string StreamlabsRedirectUri = ConfigurationManager.AppSettings["Streamlabs-RedirectUri"];
        public static string StreamlabsScope = ConfigurationManager.AppSettings["Streamlabs-Scope"];
    }
}

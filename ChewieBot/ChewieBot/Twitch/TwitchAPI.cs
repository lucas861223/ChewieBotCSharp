using ChewieBot.Config;
using ChewieBot.Database.Model;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Twitch
{
    /// <summary>
    /// Sets up and wraps the TwitchLib TwitchAPI class.
    /// </summary>
    public class TwitchAPI : ITwitchApi
    {
        private TwitchLib.Api.TwitchAPI api;
        private IUserService userService;

        public TwitchAPI(IUserService userService)
        {
            this.api = new TwitchLib.Api.TwitchAPI();
            this.api.Settings.ClientId = AppConfig.TwitchClientId;
            this.api.Settings.AccessToken = AppConfig.TwitchAccessToken;

            this.userService = userService;
        }

        public List<User> GetChatters()
        {
            var userList = this.api.Undocumented.GetChattersAsync(AppConfig.TwitchChannel).Result;
            var dbUserList = new List<User>();
            foreach (var user in userList)
            {
                var dbUser = this.userService.GetUser(user.Username);
                if (dbUser != null)
                {
                    dbUserList.Add(dbUser);
                }
                else
                {
                    dbUser = new User() { Username = user.Username };
                    this.userService.SetUser(dbUser);
                    dbUserList.Add(dbUser);
                }
            }

            return dbUserList;
        }
    }
}

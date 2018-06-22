using ChewieBot.AppStart;
using ChewieBot.Enums;
using ChewieBot.Models;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ScriptingAPI.Services
{
    public static class SongQueueService
    {
        private static ISongQueueService songService = UnityConfig.Resolve<ISongQueueService>();
        private static IUserService userService = UnityConfig.Resolve<IUserService>();

        public static ScriptServiceResponse AddSong(string username, string url)
        {
            var user = userService.GetUser(username);
            var song = songService.AddNewSong(url, user, SongRequestType.Raffle);
            var response = new ScriptServiceResponse();
            response.ResultStatus = ScriptServiceResult.SUCCESS;
            response.Message = $"Song {song.Title} added to queue.";
            return response;
        }
    }
}

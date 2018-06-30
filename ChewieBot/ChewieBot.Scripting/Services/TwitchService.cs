using ChewieBot.AppStart;
using ChewieBot.Enums;
using ChewieBot.Models;
using ChewieBot.ScriptingEngine;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ScriptingAPI.Services
{
    public static class TwitchService
    {
        private static ITwitchService twitchService = UnityConfig.Resolve<ITwitchService>();

        static TwitchService()
        {
            twitchService.InitializeClient();
        }

        public static ScriptServiceResponse SendMessage(string message)
        {
            var response = new ScriptServiceResponse();
            response.ResultStatus = ScriptServiceResult.SUCCESS;

            twitchService.SendMessage(message);
            return response;
        }
    }
}

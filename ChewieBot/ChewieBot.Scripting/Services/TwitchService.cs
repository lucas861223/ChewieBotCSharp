using ChewieBot.AppStart;
using ChewieBot.Enums;
using ChewieBot.Events.TwitchPubSub;
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

        public static event EventHandler<StreamUpArgs> OnSteamUpEvent;
        public static event EventHandler<StreamDownArgs> OnStreamDownEvent;
        public static event EventHandler<ChannelSubscriptionArgs> OnChannelSubscriptionEvent;
        public static event EventHandler<BitsReceivedArgs> OnBitsReceivedEvent;
        public static event EventHandler<HostArgs> OnHostEvent;

        static TwitchService()
        {
            if (!twitchService.IsInitialized)
            {
                twitchService.InitializeClient();
            }
        }

        public static ScriptServiceResponse SendMessage(string message)
        {
            var response = new ScriptServiceResponse();
            response.ResultStatus = ScriptServiceResult.SUCCESS;

            if (!twitchService.IsConnected)
            {
                twitchService.Connect();
            }

            twitchService.SendMessage(message);
            return response;
        }
    }
}

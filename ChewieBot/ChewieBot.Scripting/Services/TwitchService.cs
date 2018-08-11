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

        public static event EventHandler<StreamUpArgs> OnStreamUpEvent;
        public static event EventHandler<StreamDownArgs> OnStreamDownEvent;
        public static event EventHandler<ChannelSubscriptionArgs> OnChannelSubscriptionEvent;
        public static event EventHandler<BitsReceivedArgs> OnBitsReceivedEvent;
        public static event EventHandler<HostArgs> OnHostEvent;

        static TwitchService()
        {
            if (!twitchService.IsInitialized)
            {
                twitchService.InitializeClient();

                twitchService.OnStreamUpEvent += StreamUpEvent;
                twitchService.OnStreamDownEvent += StreamDownEvent;
                twitchService.OnBitsReceivedEvent += BitsReceivedEvent;
                twitchService.OnChannelSubscriptionEvent += ChannelSubscriptionEvent;
                twitchService.OnHostEvent += HostEvent;
            }
        }

        private static void StreamDownEvent(object sender, StreamDownArgs e)
        {
            OnStreamDownEvent?.Invoke(sender, e);
        }

        private static void StreamUpEvent(object sender, StreamUpArgs e)
        {
            OnStreamUpEvent?.Invoke(sender, e);
        }

        private static void BitsReceivedEvent(object sender, BitsReceivedArgs e)
        {
            OnBitsReceivedEvent?.Invoke(sender, e);
        }

        private static void ChannelSubscriptionEvent(object sender, ChannelSubscriptionArgs e)
        {
            OnChannelSubscriptionEvent?.Invoke(sender, e);
        }

        private static void HostEvent(object sender, HostArgs e)
        {
            OnHostEvent?.Invoke(sender, e);
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

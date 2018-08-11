using ChewieBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Models
{
    public class ChannelSubscription
    {
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Context { get; set; }
        public string DisplayName { get; set; }
        public int Months { get; set; }
        public string RecipientDisplayName { get; set; }
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }
        public SubMessage SubMessage { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } 
        public string SubscriptionPlanName { get; set; }
        public DateTime Time { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }

        public ChannelSubscription(TwitchLib.PubSub.Models.Responses.Messages.ChannelSubscription sub)
        {
            this.ChannelId = sub.ChannelId;
            this.ChannelName = sub.ChannelName;
            this.Context = sub.Context;
            this.DisplayName = sub.DisplayName;
            this.Months = sub.Months;
            this.RecipientDisplayName = sub.RecipientDisplayName;
            this.RecipientId = sub.RecipientId;
            this.RecipientName = sub.RecipientName;
            this.SubMessage = new SubMessage(sub.SubMessage);
            this.SubscriptionPlan = (SubscriptionPlan)sub.SubscriptionPlan;
            this.SubscriptionPlanName = sub.SubscriptionPlanName;
            this.Time = sub.Time;
            this.UserId = sub.UserId;
            this.Username = sub.Username;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Models
{
    public class SubMessage
    {
        public string Message { get; set; }
        public List<Emote> Emote { get; set; }

        public SubMessage(TwitchLib.PubSub.Models.Responses.Messages.SubMessage subMessage)
        {
            this.Message = subMessage.Message;
            this.Emote = subMessage.Emotes.Select(x => new Emote(x)).ToList();
        }
    }
}

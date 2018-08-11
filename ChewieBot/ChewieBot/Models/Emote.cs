using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Models
{
    public class Emote
    {
        public int Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public Emote(TwitchLib.PubSub.Models.Responses.Messages.SubMessage.Emote emote)
        {
            this.Id = emote.Id;
            this.Start = emote.Start;
            this.End = emote.End;
        }
    }
}

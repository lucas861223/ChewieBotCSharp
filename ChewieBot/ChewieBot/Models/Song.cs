using ChewieBot.Database.Model;
using ChewieBot.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public User RequestedBy { get; set; }
        public SongRequestType RequestType { get; set; }
        public SongSourceType SourceType { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Embeddable { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Duration.Minutes} Mins {Duration.Seconds} Seconds";
        }
    }
}

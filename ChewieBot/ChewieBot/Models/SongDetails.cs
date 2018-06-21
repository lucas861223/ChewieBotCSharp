using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ChewieBot.Models
{
    public class SongDetails
    {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

using ChewieBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Events
{
    public class SongRemovedEventArgs
    {
        public Song Song { get; set; }
        public List<Song> SongList { get; set; }
    }
}

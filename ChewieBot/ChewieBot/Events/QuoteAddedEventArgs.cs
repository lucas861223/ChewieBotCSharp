using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Events
{
    public class QuoteAddedEventArgs
    {
        public Quote Quote { get; set; }
        public List<Quote> QuoteList { get; set; }
    }
}

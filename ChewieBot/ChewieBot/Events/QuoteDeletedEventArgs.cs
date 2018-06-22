using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Events
{
    public class QuoteDeletedEventArgs
    {
        public int Id { get; set; }
    }
}

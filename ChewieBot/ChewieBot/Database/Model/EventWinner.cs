using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Model
{
    public class EventWinner
    {
        [Key]
        public int Id { get; set; }
        public ChatEvent Event { get; set; }
        public User User { get; set; }
        public int Position { get; set; }
    }
}

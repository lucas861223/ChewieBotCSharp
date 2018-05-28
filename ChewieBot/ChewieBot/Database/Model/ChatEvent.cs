using ChewieBot.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Model
{
    public class ChatEvent
    {
        [Key]
        public int EventId { get; set; }
        public EventType Type { get; set; }
        public DateTime TimeFinished { get; set; }
        public int Duration { get; set; }
        public List<User> UserList { get; set; } = new List<User>();
        public bool HasStarted { get; set; } = false;
        public bool HasFinished { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}

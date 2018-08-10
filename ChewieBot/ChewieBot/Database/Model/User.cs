using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public int Points { get; set; }
        [ForeignKey("UserLevel")]
        public int UserLevelId { get; set; }
        public virtual UserLevel UserLevel { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsWatching { get; set; } = false;
        public DateTime StartedWatchingTime { get; set; }
        public DateTime LastPointUpdateTime { get; set; }
    }
}

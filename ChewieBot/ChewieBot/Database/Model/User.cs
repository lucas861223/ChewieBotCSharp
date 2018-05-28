using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public UserLevel UserLevel { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

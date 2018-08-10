using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Model
{
    public class VIPLevel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float PointMultiplier { get; set; }
        public int Rank { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}

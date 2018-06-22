using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Model
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public string QuoteText { get; set; }
        public DateTime QuoteTime { get; set; }
    }
}

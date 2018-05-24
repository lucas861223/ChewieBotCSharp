using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Commands
{
    public class Command
    {
        public List<string> Parameters { get; set; }
        public string CommandName { get; set; }
    }
}

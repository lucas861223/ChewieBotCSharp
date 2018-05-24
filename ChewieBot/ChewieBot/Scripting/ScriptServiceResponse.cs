using ChewieBot.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Scripting
{
    public class ScriptServiceResponse
    {
        public ScriptServiceResult ResultStatus { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}

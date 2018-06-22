﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChewieBot.Enums;

namespace ChewieBot.Commands
{
    public class CommandResponse
    {
        public string Message { get; set; }
        public string ResponseType { get; set; }

        public CommandResponse(string message)
        {
            this.Message = message;
        }
    }
}

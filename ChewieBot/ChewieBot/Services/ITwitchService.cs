﻿using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface ITwitchService
    {
        void Initialize();
        void Connect();
        void Disconnect();
        IEnumerable<User> GetCurrentUsers();
    }
}

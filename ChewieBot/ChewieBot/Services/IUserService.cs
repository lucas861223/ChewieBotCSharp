﻿using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IUserService
    {
        User SetUser(User user);
        User GetUser(int id);
        User GetUser(string username);
    }
}
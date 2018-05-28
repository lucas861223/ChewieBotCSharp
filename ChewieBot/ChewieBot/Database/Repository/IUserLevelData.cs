using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IUserLevelData
    {
        UserLevel SetUserLevel(UserLevel userLevel);
        UserLevel GetUserLevel(string name);
        void DeleteUserLevel(string name);
        void DeleteUserLevel(UserLevel userLevel);
    }
}

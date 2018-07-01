using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IUserLevelService
    {
        UserLevel Get(string name);
        List<UserLevel> GetAll();
        UserLevel Set(UserLevel userLevel);
        void Delete(UserLevel userLevel);
        void Delete(string name);
    }
}

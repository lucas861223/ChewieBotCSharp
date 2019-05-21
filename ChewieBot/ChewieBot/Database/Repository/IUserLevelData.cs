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
        UserLevel Set(UserLevel userLevel);
        List<UserLevel> Set(List<UserLevel> userLevels);
        UserLevel Get(string name);
        List<UserLevel> GetAll();
        void Delete(string name);
        void Delete(UserLevel userLevel);
    }
}

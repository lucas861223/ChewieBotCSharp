using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IUserData
    {
        User Set(User user);
        List<User> Set(List<User> user);
        User Get(int id);
        User Get(string username);
        List<User> Get(List<string> usernames);
        void Delete(string username);
        void Delete(User user);
    }
}

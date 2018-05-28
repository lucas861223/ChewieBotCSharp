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
        User SetUser(User user);
        List<User> SetUsers(List<User> user);
        User GetUser(int id);
        User GetUser(string username);
        List<User> GetUsers(List<string> usernames);
        void DeleteUser(string username);
        void DeleteUser(User user);
    }
}

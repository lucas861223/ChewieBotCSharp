using ChewieBot.Database.Model;
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
        List<User> SetUsers(List<User> userList);
        User GetUser(string username);
        List<User> GetUsers(List<string> usernames);
        double GetPointsForUser(string username);
        void AddPointsForUser(string username, int points);
        void RemovePointsForUser(string username, int points);
        void SetCurrentlyWatchingUsers(List<User> users);
        void UserJoined(User user);
        void UserLeft(User user);
        bool IsUserWatching(string username);
        User AddNewUser(string username, string userLevel);
    }
}

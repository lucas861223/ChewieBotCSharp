using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Database.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class UserService : IUserService
    {
        private IUserData userData;

        public UserService(IUserData userData)
        {
            this.userData = userData;
        }

        public User SetUser(User user)
        {
            return this.userData.SetUser(user);
        }

        public User GetUser(string username)
        {
            return this.userData.GetUser(username);
        }

        public int GetPointsForUser(string username)
        {
            var user = this.GetUser(username);
            if (user != null)
            {
                return user.Points;
            }
            return -1;
        }

        public bool AddPointsForUser(string username, int points)
        {
            var user = this.GetUser(username);
            if (user != null)
            {
                user.Points += points;
                this.SetUser(user);
                return true;
            }
            return false;
        }
    }
}

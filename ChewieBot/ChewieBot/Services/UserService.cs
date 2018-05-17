using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Database.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
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

        public User GetUser(int id)
        {
            return this.userData.GetUser(id);
        }

        public User GetUser(string username)
        {
            return this.userData.GetUser(username);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;

namespace ChewieBot.Database.Repository.Implementation
{
    public class UserRepository : IUserData
    {
        public User SetUser(User user)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.Users.FirstOrDefault(x => x.Id == user.Id);
                if (record == null)
                {
                    record = user;
                    context.Users.Add(record);
                    context.SaveChanges();
                }

                return record;
            }
        }

        public User GetUser(int id)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users.FirstOrDefault(x => x.Id == id);
            }
        }

        public User GetUser(string username)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users.FirstOrDefault(x => x.Username == username);
            }
        }
    }
}

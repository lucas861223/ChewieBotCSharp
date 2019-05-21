using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Constants.SettingsConstants;
using ChewieBot.Database.Model;

namespace ChewieBot.Database.Repository.Implementation
{
    public class UserRepository : IUserData
    {
        public User Set(User user)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.Users.Find(user.Id);
                if (record == null)
                {
                    record = user;
                    var level = context.UserLevels.First(x => x.Name == UserLevelSettings.ViewerUserLevelName);
                    var vipLevel = context.VIPLevels.First(x => x.Name == VIPLevelSettings.NoVIPLevelName);
                    record.UserLevel = level;
                    record.VIPLevel = vipLevel;
                    record.JoinedDate = DateTime.Now;
                    record.Points = 0;
                    context.VIPLevels.Attach(vipLevel);
                    context.UserLevels.Attach(level);
                    context.Users.Add(record);
                }
                else
                {
                    var level = record.UserLevel;
                    var vipLevel = record.VIPLevel;
                    context.VIPLevels.Attach(vipLevel);
                    context.UserLevels.Attach(level);
                    context.Entry(record).CurrentValues.SetValues(user);
                }

                context.SaveChanges();

                return record;
            }
        }

        public User Get(int id)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users.Include("UserLevel").Include("VIPLevel").FirstOrDefault(x => x.Id == id);
            }
        }

        public User Get(string username)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users.Include("UserLevel").Include("VIPLevel").FirstOrDefault(x => x.Username == username);
            }
        }

        public List<User> Get(List<string> usernames)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users.Include("UserLevel").Include("VIPLevel").Where(x => usernames.Contains(x.Username)).ToList();
            }
        }

        public void Delete(string username)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.Users.FirstOrDefault(x => x.Username == username && !x.IsDeleted);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public void Delete(User user)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.Users.Find(user.Id);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public List<User> Set(List<User> userList)
        {
            using (var context = new DatabaseContext())
            {
                var recordList = new List<User>();
                foreach (var user in userList)
                {
                    var record = context.Users.FirstOrDefault(x => x.Username == user.Username);
                    if (record == null)
                    {
                        record = user;
                        var level = record.UserLevel;
                        var vipLevel = record.VIPLevel;
                        context.VIPLevels.Attach(vipLevel);
                        context.UserLevels.Attach(level);
                        context.Users.Add(record);
                        recordList.Add(record);
                    }
                    else
                    {
                        var level = record.UserLevel;
                        context.UserLevels.Attach(level);
                        var vipLevel = record.VIPLevel;
                        context.VIPLevels.Attach(vipLevel);
                        context.Entry(record).CurrentValues.SetValues(user);
                        recordList.Add(record);
                    }
                }

                context.SaveChanges();
                return recordList;
            }
        }
    }
}

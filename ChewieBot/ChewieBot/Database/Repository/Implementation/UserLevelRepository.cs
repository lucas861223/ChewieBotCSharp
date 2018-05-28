using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;

namespace ChewieBot.Database.Repository.Implementation
{
    public class UserLevelRepository : IUserLevelData
    {
        public void DeleteUserLevel(string name)
        {
            using (var context = new DatabaseContext())
            {
                var record = this.GetUserLevel(name);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteUserLevel(UserLevel userLevel)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.UserLevels.Find(userLevel.Id);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public UserLevel GetUserLevel(string name)
        {
            using (var context = new DatabaseContext())
            {
                return context.UserLevels.FirstOrDefault(x => x.Name == name && !x.IsDeleted);
            }
        }

        public UserLevel SetUserLevel(UserLevel userLevel)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.UserLevels.Find(userLevel.Id);
                if (record == null)
                {
                    record = userLevel;
                    context.UserLevels.Add(record);
                }
                else
                {
                    context.Entry(record).CurrentValues.SetValues(userLevel);
                }

                context.SaveChanges();
                return record;
            }
        }
    }
}

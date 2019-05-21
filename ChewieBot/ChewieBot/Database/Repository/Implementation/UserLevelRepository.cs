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
        public void Delete(string name)
        {
            using (var context = new DatabaseContext())
            {
                var record = this.Get(name);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public void Delete(UserLevel userLevel)
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

        public UserLevel Get(string name)
        {
            using (var context = new DatabaseContext())
            {
                return context.UserLevels.FirstOrDefault(x => x.Name == name && !x.IsDeleted);
            }
        }

        public List<UserLevel> GetAll()
        {
            using (var context = new DatabaseContext())
            {
                return context.UserLevels.Where(x => !x.IsDeleted).ToList();
            }
        }

        public UserLevel Set(UserLevel userLevel)
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

        public List<UserLevel> Set(List<UserLevel> userLevels)
        {
            var returnRecords = new List<UserLevel>();
            using (var context = new DatabaseContext())
            {
                foreach (var userLevel in userLevels)
                {
                    // Inefficient due to calling SaveChanges() on every record, but it's not a huge deal here and 
                    // it's easier to just call the existing function.
                    returnRecords.Add(this.Set(userLevel));
                }
            }
            return returnRecords;
        }
    }
}

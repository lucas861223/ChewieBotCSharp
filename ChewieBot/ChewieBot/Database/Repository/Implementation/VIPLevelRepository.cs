using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository.Implementation
{
    public class VIPLevelRepository : IVIPLevelData
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

        public void Delete(VIPLevel userLevel)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.VIPLevels.Find(userLevel.Id);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public VIPLevel Get(string name)
        {
            using (var context = new DatabaseContext())
            {
                return context.VIPLevels.FirstOrDefault(x => x.Name == name && !x.IsDeleted);
            }
        }

        public List<VIPLevel> GetAll()
        {
            using (var context = new DatabaseContext())
            {
                return context.VIPLevels.Where(x => !x.IsDeleted).ToList();
            }
        }

        public VIPLevel Set(VIPLevel vipLevel)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.VIPLevels.Find(vipLevel.Id);
                if (record == null)
                {
                    record = vipLevel;
                    context.VIPLevels.Add(record);
                }
                else
                {
                    context.Entry(record).CurrentValues.SetValues(vipLevel);
                }

                context.SaveChanges();
                return record;
            }
        }
    }
}

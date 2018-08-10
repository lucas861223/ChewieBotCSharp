using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository.Implementation
{
    public class ModLevelRepository : IModLevelData
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

        public void Delete(ModLevel userLevel)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.ModLevels.Find(userLevel.Id);
                if (record != null)
                {
                    record.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public ModLevel Get(string name)
        {
            using (var context = new DatabaseContext())
            {
                return context.ModLevels.FirstOrDefault(x => x.Name == name && !x.IsDeleted);
            }
        }

        public List<ModLevel> GetAll()
        {
            using (var context = new DatabaseContext())
            {
                return context.ModLevels.Where(x => !x.IsDeleted).ToList();
            }
        }

        public ModLevel Set(ModLevel modLevel)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.ModLevels.Find(modLevel.Id);
                if (record == null)
                {
                    record = modLevel;
                    context.ModLevels.Add(record);
                }
                else
                {
                    context.Entry(record).CurrentValues.SetValues(modLevel);
                }

                context.SaveChanges();
                return record;
            }
        }
    }
}

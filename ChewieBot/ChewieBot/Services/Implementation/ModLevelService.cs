using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class ModLevelService : IModLevelService
    {
        private IModLevelData modLevelData;

        public ModLevelService(IModLevelData modLevelData)
        {
            this.modLevelData = modLevelData;
        }

        public void Delete(ModLevel modLevel)
        {
            this.modLevelData.Delete(modLevel);
        }

        public void Delete(string name)
        {
            this.modLevelData.Delete(name);
        }

        public ModLevel Get(string name)
        {
            return this.modLevelData.Get(name);
        }

        public List<ModLevel> GetAll()
        {
            return this.modLevelData.GetAll();
        }

        public ModLevel Set(ModLevel modLevel)
        {
            return this.modLevelData.Set(modLevel);
        }
    }
}

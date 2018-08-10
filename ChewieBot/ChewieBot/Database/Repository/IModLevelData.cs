using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IModLevelData
    {
        ModLevel Set(ModLevel modLevel);
        ModLevel Get(string name);
        List<ModLevel> GetAll();
        void Delete(string name);
        void Delete(ModLevel modLevel);
    }
}

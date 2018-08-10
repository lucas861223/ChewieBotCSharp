using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IModLevelService
    {
        ModLevel Get(string name);
        List<ModLevel> GetAll();
        ModLevel Set(ModLevel modLevel);
        void Delete(ModLevel modLevel);
        void Delete(string name);
    }
}

using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IVIPLevelData
    {
        VIPLevel Set(VIPLevel vipLevel);
        VIPLevel Get(string name);
        List<VIPLevel> GetAll();
        void Delete(string name);
        void Delete(VIPLevel vipLevel);
    }
}

using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IVIPLevelService
    {
        VIPLevel Get(string name);
        List<VIPLevel> GetAll();
        VIPLevel Set(VIPLevel vipLevel);
        void Delete(VIPLevel vipLevel);
        void Delete(string name);
    }
}

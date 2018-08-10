using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class VIPLevelService : IVIPLevelService
    {
        private IVIPLevelData vipLevelData;

        public VIPLevelService(IVIPLevelData vipLevelData)
        {
            this.vipLevelData = vipLevelData;
        }

        public void Delete(VIPLevel vipLevel)
        {
            this.vipLevelData.Delete(vipLevel);
        }

        public void Delete(string name)
        {
            this.vipLevelData.Delete(name);
        }

        public VIPLevel Get(string name)
        {
            return this.vipLevelData.Get(name);
        }

        public List<VIPLevel> GetAll()
        {
            return this.vipLevelData.GetAll();
        }

        public VIPLevel Set(VIPLevel vipLevel)
        {
            return this.vipLevelData.Set(vipLevel);
        }
    }
}

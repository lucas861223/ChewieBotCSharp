using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class UserLevelService : IUserLevelService
    {
        private IUserLevelData userLevelData;

        public UserLevelService(IUserLevelData userLevelData)
        {
            this.userLevelData = userLevelData;
        }

        public void Delete(UserLevel userLevel)
        {
            this.userLevelData.Delete(userLevel);
        }

        public void Delete(string name)
        {
            this.userLevelData.Delete(name);
        }

        public UserLevel Get(string name)
        {
            return this.userLevelData.Get(name);
        }

        public List<UserLevel> GetAll()
        {
            return this.userLevelData.GetAll();
        }

        public UserLevel Set(UserLevel userLevel)
        {
            return this.userLevelData.Set(userLevel);
        }
    }
}

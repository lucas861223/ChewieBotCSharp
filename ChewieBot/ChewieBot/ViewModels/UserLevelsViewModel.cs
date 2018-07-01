using ChewieBot.Database.Model;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ViewModels
{
    public class UserLevelsViewModel
    {
        public List<UserLevel> LevelList { get; set; }
        private IUserLevelService userLevelService;

        public UserLevelsViewModel(IUserLevelService userLevelService)
        {
            this.userLevelService = userLevelService;
            this.LevelList = this.userLevelService.GetAll();
        }
    }
}

using ChewieBot.Config;
using ChewieBot.Constants.SettingsConstants;
using ChewieBot.Database.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class DeepbotService : IDeepbotService
    {
        private IUserService userService;
        private IUserLevelService userLevelService;
        private IModLevelService modLevelService;
        private IVIPLevelService vipLevelService;

        private List<ModLevel> modLevelList;
        private List<UserLevel> userLevelList;
        private List<VIPLevel> vipLevelList;

        public DeepbotService(IUserService userService, IUserLevelService userLevelService, IModLevelService modLevelService, IVIPLevelService vipLevelService)
        {
            this.userService = userService;
            this.userLevelService = userLevelService;
            this.modLevelService = modLevelService;
            this.vipLevelService = vipLevelService;
        }

        private void Init()
        {
            this.userLevelList = this.userLevelService.GetAll();
            this.modLevelList = this.modLevelService.GetAll();
            this.vipLevelList = this.vipLevelService.GetAll();
        }

        public void LoadDeepbotUsersFromFile()
        {
            this.Init();

            var path = AppConfig.DeepbotUserFile;
            using (var file = File.OpenText(path))
            {
                var serializer = new JsonSerializer();
                var userList = (List<dynamic>)serializer.Deserialize(file, typeof(List<dynamic>));
                userList = userList.Where(x => x.last_seen.Value > DateTime.Parse("2017-08-01")).ToList();
                this.AddUsersToDatabase(userList);
            }
        }

        private void AddUsersToDatabase(List<dynamic> userlist)
        {
            var count = 0;
            var newUserList = new List<User>();
            foreach (var user in userlist)
            {
                VIPLevel vipLevel;
                ModLevel modLevel;
                UserLevel userLevel;
                if (this.userService.GetUser(user.user.Value) == null)
                {
                    count++;
                    vipLevel = this.vipLevelList.FirstOrDefault(x => x.Rank == user.vip.Value);
                    modLevel = this.modLevelList.FirstOrDefault(x => x.Rank == user.mod.Value);

                    switch (modLevel.Rank)
                    {
                        case 1:
                        case 2:
                            {
                                userLevel = userLevelList.FirstOrDefault(x => x.Name == UserLevelSettings.ModeratorUserLevelName);
                                break;
                            }
                        case 4:
                            {
                                userLevel = userLevelList.FirstOrDefault(x => x.Name == UserLevelSettings.BotUserLevelName);
                                break;
                            }
                        case 5:
                            {
                                userLevel = userLevelList.FirstOrDefault(x => x.Name == UserLevelSettings.BroadcasterUserLevelName);
                                break;
                            }
                        case 0:
                        default:
                            {
                                userLevel = userLevelList.FirstOrDefault(x => x.Name == UserLevelSettings.ViewerUserLevelName);
                                break;
                            }
                    }

                    var newUser = new User
                    {
                        JoinedDate = user.join_date.Value,
                        ModLevel = modLevel,
                        Points = user.points.Value,
                        UserLevel = userLevel,
                        Username = user.user.Value,
                        VIPLevel = vipLevel,
                        VIPExpiryDate = user.vip_expiry.Value,
                        WatchTimeHours = user.watch_time.Value
                    };
                    newUserList.Add(newUser);
                    Console.WriteLine($"Added user {newUser.Username} -- count {count} of {userlist.Count}");
                }           
                
                if (count % 500 == 0)
                {
                    this.userService.SetUsers(newUserList);
                    newUserList.Clear();
                }
            }            
        }
    }
}

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
        private IVIPLevelService vipLevelService;

        private List<ModLevel> modLevelList;
        private List<UserLevel> userLevelList;
        private List<VIPLevel> vipLevelList;

        public DeepbotService(IUserService userService, IUserLevelService userLevelService, IVIPLevelService vipLevelService)
        {
            this.userService = userService;
            this.userLevelService = userLevelService;
            this.vipLevelService = vipLevelService;
        }

        private void Init()
        {
            this.userLevelList = this.userLevelService.GetAll();
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
                    if (user.vip.value == 10)
                    {
                        // deepbot has no vip level to 10 for some reason, instead of 0.
                        user.vip.value = 0;
                    }

                    vipLevel = this.vipLevelList.FirstOrDefault(x => x.Rank == user.vip.Value);

                    switch (user.mod.value)
                    {
                        case 1:
                            {
                                userLevel = userLevelList.FirstOrDefault(x => x.Name == UserLevelSettings.ModeratorUserLevelName);
                                break;
                            }
                        case 2:
                            {
                                userLevel = userLevelList.FirstOrDefault(x => x.Name == UserLevelSettings.SeniorModeratorUserLevelName);
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

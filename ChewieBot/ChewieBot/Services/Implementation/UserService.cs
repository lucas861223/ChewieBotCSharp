using ChewieBot.Constants;
using ChewieBot.Constants.SettingsConstants;
using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Database.Repository.Implementation;
using ChewieBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class UserService : IUserService
    {
        private IUserData userData;
        private IUserLevelService userLevelService;
        private IBotSettingService botSettingService;
        private HashSet<User> currentWatchingUsers;

        public UserService(IUserData userData, IUserLevelService userLevelService, IBotSettingService botSettingService)
        {
            this.userData = userData;
            this.userLevelService = userLevelService;
            this.botSettingService = botSettingService;
            this.currentWatchingUsers = new HashSet<User>();
        }

        /// <summary>
        /// Adds a user to the database if it doesn't exist, or updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user to add or update.</param>
        /// <returns>The user that was added or updated.</returns>
        public User SetUser(User user)
        {
            return this.userData.Set(user);
        }

        /// <summary>
        /// Adds users to the database if they don't currently exist, otherwise updates existing users in the database.
        /// </summary>
        /// <param name="userList">The list of users to add or update.</param>
        /// <returns>The list of users added and/or updated.</returns>
        public List<User> SetUsers(List<User> userList)
        {
            return this.userData.Set(userList);
        }

        /// <summary>
        /// Gets a user with their username.
        /// </summary>
        /// <param name="username">The username of the user to get.</param>
        /// <returns>The user, if it exists, or null if no user exists.</returns>
        public User GetUser(string username)
        {
            var user = this.userData.Get(username);
            return user;
        }

        /// <summary>
        /// Gets a list of users with usernames.
        /// </summary>
        /// <param name="usernames">The list of usernames to get.</param>
        /// <returns>A list containing users that are in the database that match the provided usernames. Any users that don't exist will not be included in the results.</returns>
        public List<User> GetUsers(List<string> usernames)
        {
            return this.userData.Get(usernames);
        }

        /// <summary>
        /// Gets points for a user.
        /// </summary>
        /// <param name="username">The username to get points for.</param>
        /// <returns>The number of points for the user, if the user exists, or -1 if no user exists.</returns>
        public int GetPointsForUser(string username)
        {

            var user = this.GetUser(username);
            if (user == null)
            {
                throw new UserNotExistException();
            }

            if (this.currentWatchingUsers.Any(x => x.Username == user.Username) && user.IsWatching)
            {
                user = this.AddIdlePoints(user);
            }
            return user.Points;
        }

        /// <summary>
        /// Update users points to add idle points. 
        /// </summary>
        /// <param name="user">User to add idle points to.</param>
        /// <returns>User with updated points.</returns>
        private User AddIdlePoints(User user)
        {
            var timeDifference = DateTime.Now - user.LastPointUpdateTime;
            user.LastPointUpdateTime = DateTime.Now;
            var pointRateSetting = this.botSettingService.Get(BaseSettings.PointRate.Name);
            dynamic pointRate = pointRateSetting.GetValue();
            user.Points += (int)(timeDifference.TotalMinutes * (pointRate * user.UserLevel.PointMultiplier));    // TODO: Change this to use a setting value, based on user level, etc.
            this.SetUser(user);
            return user;
        }

        /// <summary>
        /// Add points for a user.
        /// </summary>
        /// <param name="username">The username to add points to.</param>
        /// <param name="points">The number of points to add.</param>
        public void AddPointsForUser(string username, int points)
        {
            var user = this.GetUser(username);
            if (user != null)
            {
                user.Points += points;
                if (user.Points < 0)
                {
                    user.Points = 0;
                }

                this.SetUser(user);
            }
        }

        /// <summary>
        /// Remove points from a user.
        /// </summary>
        /// <param name="username">The username of the user to remove points from.</param>
        /// <param name="points">The amount of points to remove.</param>
        public void RemovePointsForUser(string username, int points)
        {
            this.AddPointsForUser(username, -points);
        }

        /// <summary>
        /// Sets the current
        /// </summary>
        /// <param name="users"></param>
        public void SetCurrentlyWatchingUsers(List<User> users)
        {
            users.ForEach(user =>
            {
                this.UserJoined(user);
            });
        }

        /// <summary>
        /// A user has started watching.
        /// </summary>
        /// <param name="user">The user that started watching.</param>
        public void UserJoined(User user)
        {
            user.IsWatching = true;
            var now = DateTime.Now;
            user.StartedWatchingTime = now;
            user.LastPointUpdateTime = now;
            this.SetUser(user);
            this.currentWatchingUsers.Add(user);
        }

        /// <summary>
        /// A user has stopped watching.
        /// </summary>
        /// <param name="user">The user that stopped watching.</param>
        public void UserLeft(User user)
        {
            this.currentWatchingUsers.Remove(user);
            user.IsWatching = false;
            user = this.AddIdlePoints(user);
        }

        public bool IsUserWatching(string username)
        {
            return this.currentWatchingUsers.Any(x => x.Username == username);
        }

        public void SetUserLevel(string username, string userLevel)
        {
            var user = this.GetUser(username);
            var level = this.userLevelService.Get(userLevel);
            if (user != null && level != null)
            {
                user.UserLevel = level;
                this.SetUser(user);
            }
        }

        public User AddNewUser(string username, string userLevel)
        {
            var newUser = this.GetUser(username);
            if (newUser == null)
            {
                var level = this.userLevelService.Get(userLevel);
                newUser = new User { Username = username, UserLevel = level };
                this.SetUser(newUser);
            }

            return newUser;
        }
    }
}

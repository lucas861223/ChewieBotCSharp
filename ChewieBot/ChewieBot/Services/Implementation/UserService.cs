using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Database.Repository.Implementation;
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

        public UserService(IUserData userData)
        {
            this.userData = userData;
        }

        /// <summary>
        /// Adds a user to the database if it doesn't exist, or updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user to add or update.</param>
        /// <returns>The user that was added or updated.</returns>
        public User SetUser(User user)
        {
            return this.userData.SetUser(user);
        }

        /// <summary>
        /// Adds users to the database if they don't currently exist, otherwise updates existing users in the database.
        /// </summary>
        /// <param name="userList">The list of users to add or update.</param>
        /// <returns>The list of users added and/or updated.</returns>
        public List<User> SetUsers(List<User> userList)
        {
            return this.userData.SetUsers(userList);
        }

        /// <summary>
        /// Gets a user with their username.
        /// </summary>
        /// <param name="username">The username of the user to get.</param>
        /// <returns>The user, if it exists, or null if no user exists.</returns>
        public User GetUser(string username)
        {
            return this.userData.GetUser(username);
        }

        /// <summary>
        /// Gets a list of users with usernames.
        /// </summary>
        /// <param name="usernames">The list of usernames to get.</param>
        /// <returns>A list containing users that are in the database that match the provided usernames. Any users that don't exist will not be included in the results.</returns>
        public List<User> GetUsers(List<string> usernames)
        {
            return this.userData.GetUsers(usernames);
        }

        /// <summary>
        /// Gets points for a user.
        /// </summary>
        /// <param name="username">The username to get points for.</param>
        /// <returns>The number of points for the user, if the user exists, or -1 if no user exists.</returns>
        public int GetPointsForUser(string username)
        {
            var user = this.GetUser(username);
            if (user != null)
            {
                return user.Points;
            }
            return -1;
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
                this.SetUser(user);
            }
        }

        public void RemovePointsForUser(string username, int points)
        {
            this.AddPointsForUser(username, -points);
        }
    }
}

using ChewieBot.AppStart;
using ChewieBot.Enum;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Scripting
{
    public static class UserService
    {
        private static IUserService userService = UnityConfig.Resolve<IUserService>();

        // TODO: Update the service to throw exceptions and handle here, instead of returning values to indicate success/failure..
        public static ScriptServiceResponse GetUser(string username)
        {
            var response = new ScriptServiceResponse();
            response.Data = userService.GetUser(username);
            if (response.Data == null)
            {
                response.ResultStatus = ScriptServiceResult.USER_NOT_EXIST;
                response.Message = $"Unable to find user with name: {username}";
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.SUCCESS;
            }

            return response;
        }

        public static ScriptServiceResponse GetPointsForUser(string username)
        {
            var response = new ScriptServiceResponse();
            int? data = userService.GetPointsForUser(username);
            response.Data = data > 0 ? data : null;
            if (response.Data == null)
            {
                response.ResultStatus = ScriptServiceResult.ERROR;
                response.Message = $"Unable to get points for user with name: ${username}";
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.SUCCESS;
            }

            return response;
        }

        public static ScriptServiceResponse AddPointsForUser(string username, int points)
        {
            var response = new ScriptServiceResponse();
            if (!userService.AddPointsForUser(username, points))
            {
                response.ResultStatus = ScriptServiceResult.USER_NOT_EXIST;
                response.Message = $"Unable to add points to user with name: ${username} as the user does not exist.";
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.SUCCESS;
            }

            return response;
        }
    }
}

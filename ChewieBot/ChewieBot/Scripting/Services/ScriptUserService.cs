using ChewieBot.Database.Model;
using ChewieBot.Enum;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Scripting.Services
{
    /// <summary>
    /// Exposes certain functions of the UserService to the scripting engine.
    /// </summary>
    public class ScriptUserService
    {
        private IUserService userService;

        public ScriptUserService(IUserService userService)
        {
            this.userService = userService;
        }

        // TODO: Update the service to throw exceptions and handle here, instead of returning values to indicate success/failure..
        public ScriptServiceResponse GetUser(string username)
        {
            var response = new ScriptServiceResponse();
            response.Data = this.userService.GetUser(username);
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

        public ScriptServiceResponse GetPointsForUser(string username)
        {
            var response = new ScriptServiceResponse();
            int? data = this.userService.GetPointsForUser(username);
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

        public ScriptServiceResponse AddPointsForUser(string username, string points)
        {
            var response = new ScriptServiceResponse();
            int pointsInt;
            if (Int32.TryParse(points, out pointsInt))
            {
                if (!this.userService.AddPointsForUser(username, Int32.Parse(points)))
                {
                    response.ResultStatus = ScriptServiceResult.USER_NOT_EXIST;
                    response.Message = $"Unable to add points to user with name: ${username} as the user does not exist.";
                }
                else
                {
                    response.ResultStatus = ScriptServiceResult.SUCCESS;
                }
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.ERROR;
                response.Message = $"Unable to parse points to an integer. Points needs to be a number.";
            }

            return response;
        }
    }
}

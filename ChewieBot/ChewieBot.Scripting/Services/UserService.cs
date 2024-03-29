﻿using ChewieBot.AppStart;
using ChewieBot.Enums;
using ChewieBot.Exceptions;
using ChewieBot.Models;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ScriptingAPI.Services
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
            try
            {
                double data = userService.GetPointsForUser(username);
                response.Data = data;
                response.ResultStatus = ScriptServiceResult.SUCCESS;              
            }
            catch (UserNotExistException)
            {
                response.ResultStatus = ScriptServiceResult.USER_NOT_EXIST;
                response.Message = $"Unable to get points for user with name: ${username}";
            }

            return response;
        }

        public static ScriptServiceResponse AddPointsForUser(string username, string points)
        {
            var response = new ScriptServiceResponse();
            if (int.TryParse(points, out int pointsInt))
            {
                userService.AddPointsForUser(username, pointsInt);
                response.ResultStatus = ScriptServiceResult.SUCCESS;
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.PARSE_ERROR;
                response.Message = "Could not parse points to an integer.";
            }

            return response;
        }
    }
}

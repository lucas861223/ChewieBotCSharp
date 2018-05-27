﻿using ChewieBot.AppStart;
using ChewieBot.Database.Model;
using ChewieBot.Enum;
using ChewieBot.Scripting;
using ChewieBot.Scripting.Events;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ScriptingAPI
{
    /*TODO Need to work out a decent way for how to handle function call status/error handling/etc, as the core services using void and not throwing exceptions or returning errors is not really all that useful.
     * Options are throw exceptions, then add try/catch everywhere, and update the return response based on those, or have the core services also return a response object.
     */
    public static class ChatEventService
    {
        private static IChatEventService chatEventService = UnityConfig.Resolve<IChatEventService>();

        public static event EventHandler<EventStartedEventArgs> OnEventStarted;
        public static event EventHandler<EventEndedEventArgs> OnEventEnded;

        static ChatEventService()
        {
            chatEventService.OnEventStarted += EventStarted;
            chatEventService.OnEventEnded += EventEnded;
        }

        private static void EventEnded(object sender, EventEndedEventArgs e)
        {
            OnEventEnded?.Invoke(null, e);
        }

        private static void EventStarted(object sender, EventStartedEventArgs e)
        {
            OnEventStarted?.Invoke(null, e);
        }

        public static ScriptServiceResponse AddUserToEvent(string eventId, string username)
        {
            var response = new ScriptServiceResponse();
            if (int.TryParse(eventId, out int id))
            {
                chatEventService.AddUser(id, username);
                response.ResultStatus = ScriptServiceResult.SUCCESS;
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.PARSE_ERROR;
                response.Message = $"Unable to parse variable eventId -- {eventId} -- to an integer.";
            }

            return response;
        }

        public static ScriptServiceResponse AddUserToCurrentEvent(string username)
        {
            var response = new ScriptServiceResponse();
            chatEventService.AddUserToCurrentEvent(username);
            response.ResultStatus = ScriptServiceResult.SUCCESS;
            return response;
        }

        public static ScriptServiceResponse CreateNewEvent(string eventType, string eventDuration)
        {
            var response = new ScriptServiceResponse();
            if (System.Enum.TryParse(eventType, out Enum.EventType type) && int.TryParse(eventDuration, out int duration))
            {
                response.ResultStatus = ScriptServiceResult.SUCCESS;
                response.Data = chatEventService.CreateNewEvent(type, duration * 1000); // duration is in seconds, so multiply by 1000 to get ms for timers.
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.PARSE_ERROR;
                response.Message = $"Unable to parse variable eventType -- {eventType} -- to an EventType, or eventDuration -- {eventDuration} -- to an integer.";
            }

            return response;
        }

        public static ScriptServiceResponse StartEvent(string eventId)
        {
            var response = new ScriptServiceResponse();
            if (int.TryParse(eventId, out int id))
            {
                response.ResultStatus = ScriptServiceResult.SUCCESS;
                chatEventService.StartEvent(id);
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.PARSE_ERROR;
                response.Message = $"Could not parse variable eventId -- {eventId} -- to an integer.";
            }

            return response;
        }

        public static ScriptServiceResponse StartEvent(int eventId)
        {
            var response = new ScriptServiceResponse();
            chatEventService.StartEvent(eventId);
            response.ResultStatus = ScriptServiceResult.SUCCESS;
            return response;
        }
    }
}

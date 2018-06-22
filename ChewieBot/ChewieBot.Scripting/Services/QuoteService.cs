using ChewieBot.AppStart;
using ChewieBot.Database.Model;
using ChewieBot.Models;
using ChewieBot.ScriptingEngine;
using ChewieBot.Services;
using ChewieBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ScriptingAPI.Services
{
    public static class QuoteService
    {
        private static IQuoteService quoteService = UnityConfig.Resolve<IQuoteService>();

        public static ScriptServiceResponse AddQuote(string username, string quotedUser, string quoteText)
        {
            var response = new ScriptServiceResponse();
            var quote = quoteService.AddQuote(quotedUser, quoteText);
            if (quote != null)
            {
                response.ResultStatus = ScriptServiceResult.SUCCESS;
                response.Message = "Quote added.";
                response.Data = quote;
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.ERROR;
                response.Message = "Error attempting to add quote.";
            }
            return response;
        }

        public static ScriptServiceResponse GetQuote(string id)
        {
            var response = new ScriptServiceResponse();
            var quote = quoteService.GetQuote(Int32.Parse(id));
            if (quote != null)
            {
                response.ResultStatus = ScriptServiceResult.SUCCESS;
                response.Data = quote;
            }
            else
            {
                response.ResultStatus = ScriptServiceResult.ERROR;
                response.Message = $"Could not find quote with id: {id}";
            }
            return response;
        }

        public static ScriptServiceResponse DeleteQuote(string id)
        {
            var response = new ScriptServiceResponse();
            quoteService.DeleteQuote(Int32.Parse(id));
            response.ResultStatus = ScriptServiceResult.SUCCESS;
            response.Message = $"Quote deleted with id: {id}";
            return response;
            
        }
    }
}

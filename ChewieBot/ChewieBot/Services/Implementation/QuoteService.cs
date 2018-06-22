using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;

namespace ChewieBot.Services.Implementation
{
    public class QuoteService : IQuoteService
    {
        private IQuoteData quoteData;
        private IUserService userService;

        public QuoteService(IQuoteData quoteData, IUserService userService)
        {
            this.quoteData = quoteData;
            this.userService = userService;
        }

        public Quote AddQuote(string username, string quoteText)
        {
            var user = this.userService.GetUser(username);
            if (user != null)
            {
                return quoteData.AddQuote(user, quoteText);
            }
            return null;
        }

        public void DeleteQuote(int id)
        {
            quoteData.DeleteQuote(id);
        }

        public Quote GetQuote(int id)
        {
            return quoteData.GetQuote(id);
        }
    }
}

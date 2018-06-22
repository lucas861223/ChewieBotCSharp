using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IQuoteData
    {
        Quote GetQuote(int id);
        IEnumerable<Quote> GetQuotesForUser(string username);
        IEnumerable<Quote> GetAllQuotes();
        void DeleteQuote(int id);
        Quote AddQuote(User user, string quote);
    }
}

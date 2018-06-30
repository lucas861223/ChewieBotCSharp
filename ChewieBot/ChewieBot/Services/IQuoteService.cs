using ChewieBot.Database.Model;
using ChewieBot.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IQuoteService
    {
        Quote GetQuote(int id);
        Quote AddQuote(string username, string quoteText);
        void DeleteQuote(int id);
        IEnumerable<Quote> GetAllQuotes();
        void Initialize();

        event EventHandler<QuoteAddedEventArgs> QuoteAddedEvent;
        event EventHandler<QuoteDeletedEventArgs> QuoteDeletedEvent;
    }
}

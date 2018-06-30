using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.Events;

namespace ChewieBot.Services.Implementation
{
    public class QuoteService : IQuoteService
    {
        private IQuoteData quoteData;
        private IUserService userService;

        public event EventHandler<QuoteAddedEventArgs> QuoteAddedEvent;
        public event EventHandler<QuoteDeletedEventArgs> QuoteDeletedEvent;

        private List<Quote> quoteList;

        public QuoteService(IQuoteData quoteData, IUserService userService)
        {
            this.quoteData = quoteData;
            this.userService = userService;
        }

        public void Initialize()
        {
            this.quoteList = this.quoteData.GetAllQuotes().ToList();
        }

        public IEnumerable<Quote> GetAllQuotes()
        {
            return this.quoteList;
        }

        public Quote AddQuote(string username, string quoteText)
        {
            var user = this.userService.GetUser(username);
            if (user != null)
            {
                var quote = quoteData.AddQuote(user, quoteText);
                if (quote != null)
                {
                    this.quoteList.Add(quote);
                    this.QuoteAddedEvent?.Invoke(this, new QuoteAddedEventArgs { Quote = quote, QuoteList = this.quoteList });
                    return quote;
                }
            }
            return null;
        }

        public void DeleteQuote(int id)
        {            
            var quoteToDelete = this.quoteList.FirstOrDefault(x => x.Id == id);
            if (quoteToDelete != null)
            {
                this.quoteList.Remove(quoteToDelete);
                this.QuoteDeletedEvent?.Invoke(this, new QuoteDeletedEventArgs { Id = id });
                quoteData.DeleteQuote(id);
            }            
        }

        public Quote GetQuote(int id)
        {
            return quoteData.GetQuote(id);
        }
    }
}

using ChewieBot.Database.Model;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ViewModels
{
    public class QuotesViewModel
    {
        private IQuoteService quoteService;

        public QuotesViewModel(IQuoteService quoteService)
        {
            this.quoteService = quoteService;
            this.QuoteList = quoteService.GetAllQuotes().ToList();
        }

        public List<Quote> QuoteList { get; set; }
    }
}

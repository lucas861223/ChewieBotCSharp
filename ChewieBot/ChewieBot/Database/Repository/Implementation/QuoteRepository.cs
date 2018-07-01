using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChewieBot.Database.Model;

namespace ChewieBot.Database.Repository.Implementation
{
    public class QuoteRepository : IQuoteData
    {
        public Quote Set(User user, string quoteText)
        {
            using (var context = new DatabaseContext())
            {
                var quote = new Quote() { User = user, QuoteText = quoteText, QuoteTime = DateTime.Now };
                context.Quotes.Add(quote);
                context.SaveChanges();
                return quote;
            }
        }

        public void Delete(int id)
        {
            using (var context = new DatabaseContext())
            {
                var quote = context.Quotes.Include("User").FirstOrDefault(x => x.Id == id);
                if (quote != null)
                {
                    context.Quotes.Remove(quote);
                    context.SaveChanges();
                }
            }
        }

        public Quote Get(int id)
        {
            using (var context = new DatabaseContext())
            {
                return context.Quotes.Include("User").FirstOrDefault(x => x.Id == id);
            }
        }

        public IEnumerable<Quote> GetAllForUser(string username)
        {
            using (var context = new DatabaseContext())
            {
                return context.Quotes.Include("User").Where(x => x.User.Username == username);
            }
        }

        public IEnumerable<Quote> GetAll()
        {
            using (var context = new DatabaseContext())
            {
                return context.Quotes.Include("User").ToList();
            }
        }
    }
}

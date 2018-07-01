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
        Quote Get(int id);
        IEnumerable<Quote> GetAllForUser(string username);
        IEnumerable<Quote> GetAll();
        void Delete(int id);
        Quote Set(User user, string quote);
    }
}

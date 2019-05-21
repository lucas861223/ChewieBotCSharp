using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository
{
    public interface IKeyValueData
    {
        KeyValuePair<string, string> Get(string key);
        KeyValuePair<string, string> Set(string key, string value);
        KeyValuePair<string, string> Set(KeyValuePair<string, string> kvp);
    }
}

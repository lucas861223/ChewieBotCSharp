using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface IConfigService
    {
        string Get(string key);
        KeyValuePair<string, string> Set(string key, string value);
    }
}

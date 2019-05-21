using ChewieBot.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class ConfigService : IConfigService
    {
        private IKeyValueData configData;
        public ConfigService(IKeyValueData configData)
        {
            this.configData = configData;
        }

        public string Get(string key)
        {
            var item = this.configData.Get(key);
            return item.Value;
        }

        public KeyValuePair<string, string> Set(string key, string value)
        {
            var item = this.configData.Set(key, value);
            return item;
        }
    }
}

using ChewieBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Repository.Implementation
{
    public class KeyValueRepository : IKeyValueData
    {
        public KeyValuePair<string, string> Get(string key)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.Config.FirstOrDefault(x => x.Key == key);
                if (record != null)
                {
                    return new KeyValuePair<string, string>(record.Key, record.Value);
                }
                else return new KeyValuePair<string, string>();
            }
        }

        public KeyValuePair<string, string> Set(string key, string value)
        {
            using (var context = new DatabaseContext())
            {
                var record = context.Config.FirstOrDefault(x => x.Key == key);
                if (record == null)
                {
                    var kvp = new KeyValue { Key = key, Value = value };
                    context.Config.Add(kvp);
                    context.SaveChanges();
                    return new KeyValuePair<string, string>(key, value);
                }
                else
                {
                    var kvp = record;
                    record.Value = value;
                    context.Entry(record).CurrentValues.SetValues(kvp);
                    context.SaveChanges();
                    return new KeyValuePair<string, string>(record.Key, record.Value);
                }
            }
        }

        public KeyValuePair<string, string> Set(KeyValuePair<string, string> kvp)
        {
            return this.Set(kvp.Key, kvp.Value);
        }
    }
}

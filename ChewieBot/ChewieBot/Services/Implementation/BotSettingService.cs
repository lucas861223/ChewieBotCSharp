using ChewieBot.Config;
using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class BotSettingService : IBotSettingService
    {
        private IBotSettingData settingData;
        private Dictionary<string, dynamic> cachedSettings;        
        private Dictionary<string, DateTime> cacheUpdatedTimes;
        private int cacheDuration;

        public BotSettingService(IBotSettingData settingData)
        {
            this.settingData = settingData;
            this.cachedSettings = new Dictionary<string, dynamic>();
            this.cacheUpdatedTimes = new Dictionary<string, DateTime>();
            this.cacheDuration = AppConfig.SettingsCacheDuration;
        }

        public void Delete(string name)
        {
            this.settingData.Delete(name);
        }

        public void Delete(BotSetting setting)
        {
            this.settingData.Delete(setting);
        }

        private BotSetting Get(string name)
        {
            return this.settingData.Get(name);
        }

        public BotSetting Set(string name, string description, object value, Type type)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, value);
            var setting = new BotSetting { Name = name, Description = description, Value = ms.ToArray(), TypeName = type.ToString() };
            return this.settingData.Set(setting);
        }

        public bool Exists(string name)
        {
            return this.Get(name) != null;
        }

        public dynamic GetValue(string name)
        {
            if (!this.cachedSettings.Keys.Contains(name) || 
                (this.cacheUpdatedTimes.Keys.Contains(name) && (DateTime.Now - this.cacheUpdatedTimes[name]).TotalSeconds >= cacheDuration))
            {
                var setting = this.Get(name);

                dynamic cachedValue = null;
                var ms = new MemoryStream();
                var bf = new BinaryFormatter();
                ms.Write(setting.Value, 0, setting.Value.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var valueObject = bf.Deserialize(ms);
                cachedValue = Convert.ChangeType(valueObject, Type.GetType(setting.TypeName));

                if (this.cacheUpdatedTimes.Keys.Contains(name))
                {
                    this.cacheUpdatedTimes[name] = DateTime.Now;
                    this.cachedSettings[name] = cachedValue;
                }
                else
                {
                    this.cacheUpdatedTimes.Add(name, DateTime.Now);
                    this.cachedSettings.Add(name, cachedValue);
                }
            }

            return this.cachedSettings[name];
        }
    }
}

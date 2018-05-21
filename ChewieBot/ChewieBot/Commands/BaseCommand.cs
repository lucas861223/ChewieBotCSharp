using ChewieBot.Database.Model;
using ChewieBot.Enum;
using ChewieBot.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Commands
{
    public abstract class BaseCommand : ICommand
    {
        [JsonIgnore]
        public User User { get; private set; }

        [JsonProperty("commandType")]
        public CommandType Type { get; private set; }

        [JsonIgnore]
        public CommandResponse Response { get; private set; }

        [JsonProperty("commandName")]
        public string Name { get; private set; }

        public BaseCommand()
        {

        }

        public void Execute()
        {

        }
    }
}

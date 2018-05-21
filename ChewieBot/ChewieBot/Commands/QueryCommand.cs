using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Commands
{
    public class QueryCommand : BaseCommand
    {
        [JsonProperty("queryData.queryField")]
        private List<CommandToken> queryParams;

        public QueryCommand()
            : base()
        {
            this.queryParams = new List<CommandToken>();
        }

        public QueryCommand(List<CommandToken> queryParams)
            : base()
        {
            this.queryParams = queryParams;
        }

        public void AddQueryParam(string token, string field, string value)
        {
            this.queryParams.Add(new CommandToken() { Token = token, Field = field, Value = value });
        }

        private CommandToken GetTokenValue(CommandToken token)
        {
            var fields = token.Field.Split('.');
            var service = fields[0];
            switch (service)
            {
                case "user":
                    {
                        // TODO Pull token value from user service.
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return token;
        }
    }
}

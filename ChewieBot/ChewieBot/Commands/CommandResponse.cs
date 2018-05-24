using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChewieBot.Commands
{
    public class CommandResponse
    {
        protected Dictionary<string, string> responseParameters;
        protected string tokenisedResponse;
        protected string response;

        public CommandResponse(string tokenisedResponse)
        {
            this.tokenisedResponse = tokenisedResponse;
            this.responseParameters = new Dictionary<string, string>();
        }

        protected void SetParam(string key, string value)
        {
            if (this.responseParameters.ContainsKey(key))
            {
                this.responseParameters[key] = value;
            }
        }

        protected string CreateResponseString(string response, Dictionary<string, string> parameters)
        {
            foreach (var param in parameters)
            {
                response = response.Replace(param.Key, param.Value);
            }

            response = response.Replace("{", "");
            response = response.Replace("}", "");

            return response;
        }

        public override string ToString()
        {
            // set value for potential caching. May remove it later.
            if (String.IsNullOrEmpty(this.response))
            {
                this.response = this.CreateResponseString(this.tokenisedResponse, this.responseParameters);
            }

            return response;
        }
    }
}

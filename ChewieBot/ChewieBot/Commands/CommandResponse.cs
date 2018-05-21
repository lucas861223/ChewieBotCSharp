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
            this.responseParameters = this.ParseTokenisedResponse(this.tokenisedResponse);
        }

        protected Dictionary<string, string> ParseTokenisedResponse(string tokenised)
        {
            var dict = new Dictionary<string, string>();
            var bracesPattern = new Regex("{[^}]*}|[^{}]+");
            var matchResult = bracesPattern.Match(tokenised);
            // Matches based on {} around text.
            if (matchResult.Success)
            {
                Match match = null;
                do
                {
                    match = matchResult.NextMatch();
                    // If the match has a {, then it's a token value, so add it to our dictionary with an empty value for now.
                    if (match != null && match.Value.IndexOf('{') > 0)
                    {
                        dict.Add(match.Value.Substring(1, match.Value.Length - 1), String.Empty);
                    }
                } while (match != null);
            }
            return dict;
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

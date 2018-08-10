using ChewieBot.ScriptingEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Models
{
    public class CommandListItem
    {
        public string CommandName { get; set; }
        public int Cost { get; set; }
        public List<CommandParameter> Parameters { get; set; }
        
        public string ParametersAsString
        {
            get
            {
                if (Parameters != null && Parameters.Count > 0)
                {
                    return string.Join(", ", Parameters.Select(x => x.Key));
                }
                else
                {
                    return "No Parameters";
                }
            }
        }
    }
}

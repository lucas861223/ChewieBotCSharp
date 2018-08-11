using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Events
{
    /// <summary>
    /// Inherited by events that will be exposed to the python scripts, so that the scripts can know the event that triggered the script.
    /// </summary>
    public abstract class BaseEvent
    {
        public string TriggeredByEvent { get; set; }
    }
}

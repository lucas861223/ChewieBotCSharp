using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Exceptions
{
    [Serializable]
    public class CommandNotExistException : Exception
    {
        public string ResourceReferenceProperty { get; set; }
        public string CommandName { get; set; }

        public CommandNotExistException()
        {
        }

        public CommandNotExistException(string message)
            : base(message)
        {

        }

        public CommandNotExistException(string message, string commandName)
            : base(message)
        {
            this.CommandName = commandName;
        }

        public CommandNotExistException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public CommandNotExistException(string message, Exception inner, string commandName)
            : base(message, inner)
        {
            this.CommandName = commandName;
        }

        protected CommandNotExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("ResourceReferenceProperty", this.ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}

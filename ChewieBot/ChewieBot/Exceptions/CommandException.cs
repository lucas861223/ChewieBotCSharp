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
    public class CommandException : Exception
    {
        public string ResourceReferenceProperty { get; set; }
        public string CommandName { get; set; }
        public bool ShouldSendToClient { get; set; }

        public CommandException()
        {
        }

        public CommandException(string message)
            : base(message)
        {

        }

        public CommandException(string message, bool sendToClient)
            : base(message)
        {
            this.ShouldSendToClient = sendToClient;
        }

        public CommandException(string message, string commandName)
            : base(message)
        {
            this.CommandName = commandName;
        }

        public CommandException(string message, string commandName, bool sendToClient)
            : base(message)
        {
            this.CommandName = commandName;
            this.ShouldSendToClient = sendToClient;
        }

        public CommandException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public CommandException(string message, Exception inner, bool sendToClient)
            : base(message, inner)
        {
            this.ShouldSendToClient = sendToClient;
        }

        public CommandException(string message, Exception inner, string commandName)
            : base(message, inner)
        {
            this.CommandName = commandName;
        }

        public CommandException(string message, Exception inner, string commandName, bool sendToClient)
            : base(message, inner)
        {
            this.CommandName = commandName;
            this.ShouldSendToClient = sendToClient;
        }

        protected CommandException(SerializationInfo info, StreamingContext context)
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

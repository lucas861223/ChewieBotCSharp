using ChewieBot.Database.Model;
using ChewieBot.ScriptingEngine;
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
    public class CommandPointsException : Exception
    {

        public string ResourceReferenceProperty { get; set; }
        public User User { get; set; }
        public Command Command { get; set; }

        public CommandPointsException()
        {
        }

        public CommandPointsException(User user, Command command)
        {
            this.User = user;
            this.Command = command;
        }

        public CommandPointsException(string message)
            : base(message)
        {

        }

        public CommandPointsException(string message, User user, Command command)
            : base(message)
        {
            this.User = user;
            this.Command = command;
        }

        public CommandPointsException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public CommandPointsException(string message, Exception inner, User user, Command command)
            : base(message, inner)
        {
            this.User = user;
            this.Command = command;
        }

        protected CommandPointsException(SerializationInfo info, StreamingContext context)
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

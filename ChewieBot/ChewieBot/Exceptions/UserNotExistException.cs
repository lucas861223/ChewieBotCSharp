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
    public class UserNotExistException : Exception
    {
        public string ResourceReferenceProperty { get; set; }

        public UserNotExistException()
        {
        }

        public UserNotExistException(string message)
            : base(message)
        {

        }

        public UserNotExistException(string message, Exception inner)
            : base(message, inner)
        {

        }

        protected UserNotExistException(SerializationInfo info, StreamingContext context)
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

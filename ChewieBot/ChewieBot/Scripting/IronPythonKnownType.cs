using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Scripting
{
    public class IronPythonKnownType : DynamicObject
    {
        public IronPythonKnownType(dynamic obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                var val = prop.GetValue(obj);
                this.Set(prop.Name, val);
            }
        }

        private Dictionary<string, object> _dict = new Dictionary<string, object>();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_dict.ContainsKey(binder.Name))
            {
                result = _dict[binder.Name];
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        private void Set(string name, object value)
        {
            _dict[name] = value;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dict[binder.Name] = value;
            return true;
        }
    }
}

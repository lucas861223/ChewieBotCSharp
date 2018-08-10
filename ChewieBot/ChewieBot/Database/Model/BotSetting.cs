using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database.Model
{
    public class BotSetting
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Value { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string TypeName { get; set; }

        public dynamic GetValue()
        {
            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            ms.Write(Value, 0, Value.Length);
            ms.Seek(0, SeekOrigin.Begin);
            var valueObject = bf.Deserialize(ms);
            return Convert.ChangeType(valueObject, Type.GetType(this.TypeName));
        }
    }
}

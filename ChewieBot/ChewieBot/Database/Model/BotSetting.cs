using System.ComponentModel.DataAnnotations;

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
    }
}

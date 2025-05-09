using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public class Tag
    {
        [Key]
        public string TagId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;

        // 对应用户
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = new User();

        // 对应心情记录
        public List<MoodRecord> MoodRecords { get; set; } = [];
    }
}

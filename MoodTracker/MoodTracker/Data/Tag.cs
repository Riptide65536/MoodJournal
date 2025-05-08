using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public class Tag
    {
        public string TagId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;

        // 多对多关系：一个标签可以属于多个心情记录
        public ICollection<MoodRecord> MoodRecords { get; set; } = new List<MoodRecord>();
    }
}

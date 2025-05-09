using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public enum EmotionType { Happy, Sad, Neutral, Anxiety, Afraid, Angry };
    public class MoodRecord
    {
        [Key]
        public string RecordId { get; set; } = Guid.NewGuid().ToString(); //记录Id

        public DateTime Datetime { get; set; } = DateTime.Now; //用户记录时间

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty; //所属用户Id
        public User User { get; set; } = new User();

        public EmotionType CurrentEmotion { get; set; } // 用户心情（枚举类型）

        public string SongName { get; set; } = string.Empty; //记录歌曲名
        public string SongLink { get; set; } = string.Empty; //记录歌曲链接

        public string Title { get; set; } = string.Empty; //用户标题
        public string Content { get; set; } = string.Empty; //用户笔记

        // 多对多关系：一个心情记录可以有多个标签
        public List<Tag> Tags { get; set; } = [];
    }
}
//数据库表结构
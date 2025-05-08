using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public class MoodRecord
    {
        public string RecordId { get; set; } = Guid.NewGuid().ToString(); //记录Id

        public DateTime Datetime { get; set; } = DateTime.Now; //用户记录时间

        public string UserId { get; set; } = string.Empty; //所属用户Id

        public string MoodId { get; set; } = string.Empty; //心情Id
        public string Mood { get; set; } = string.Empty; //用户心情

        public string Songname { get; set; } = string.Empty; //记录歌曲名
        public string SongLink { get; set; } = string.Empty; //记录歌曲链接

        public string Title { get; set; } = string.Empty; //用户标题
        public string Content { get; set; } = string.Empty; //用户笔记

        // 多对多关系：一个心情记录可以有多个标签
        public ICollection<Tag> Tags { get; set; } = [];
    }
}
//数据库表结构
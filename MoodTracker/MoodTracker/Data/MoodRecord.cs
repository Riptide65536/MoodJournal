using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Models
{
    public class MoodRecord
    {
        public string RecordId { get; set; } //记录Id

        public DateTime Datetime { get; set; } //用户记录时间

        public string UserId { get; set; } //所属用户Id

        public string MoodId { get; set; } //心情Id
        public string Mood { get; set; } //用户心情

        public string Songname { get; set; } //记录歌曲名
        public string SongLink { get; set; } // 记录歌曲链接

        public string Content { get; set; } //用户笔记
    }
}
//数据库表结构
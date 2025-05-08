using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Models
{
    public class RecordModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }
        public string MusicPath { get; set; }
        public string Mood { get; set; }      // 今日心情
        public string MusicName { get; set; } // 音乐名
    }
}

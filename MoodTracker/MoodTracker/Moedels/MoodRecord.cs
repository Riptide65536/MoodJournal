using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Moedels
{
    public class MoodRecord
    {
        public string ID { get; set; }  //用户Id

        public string Mood { get; set; } //用户心情

        public string Note { get; set; } //用户笔记
        public string Song { get; set; } //记录歌曲名
        public DateTime Date { get; set; } //用户记录时间
    }
}
//数据库表结构
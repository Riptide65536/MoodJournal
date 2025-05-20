using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public enum EmotionType { Happy, Sad, Neutral, Anxiety, Afraid, Angry, Calm, Excited, Confused };
    public class MoodRecord
    {
        [Key]
        public string RecordId { get; set; } = Guid.NewGuid().ToString(); //记录Id  

        public DateTime Datetime { get; set; } = DateTime.Now; //用户记录时间  

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty; //所属用户Id  
        public User User { get; set; } = new User();

        private EmotionType currentEmotion; // 用户心情（枚举类型）  
        public EmotionType CurrentEmotion
        {
            get => currentEmotion;
            set { if (currentEmotion != value) { currentEmotion = value; OnPropertyChanged(); } }
        }

        private string songname = string.Empty; //记录歌曲名  
        public string SongName
        {
            get => songname;
            set { if (songname != value) { songname = value; OnPropertyChanged(); } }
        }

        private string songLink = string.Empty; //记录歌曲链接  
        public string SongLink
        {
            get => songLink;
            set { if (songLink != value) { songLink = value; OnPropertyChanged(); } }
        }

        private string title = string.Empty; // 用户记录标题  
        public string Title
        {
            get => title;
            set { if (title != value) { title = value; OnPropertyChanged(); } }
        }

        private string content = string.Empty; //用户笔记  
        public string Content
        {
            get => content;
            set { if (content != value) { content = value; OnPropertyChanged(); } }
        }

        // 多对多关系：一个心情记录可以有多个标签  
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public event PropertyChangedEventHandler? PropertyChanged; // 将事件声明为可为 null  
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 转化为字符串
        public override string ToString()
        {
            return $"📕 {Title}";
        }
    }
}
//数据库表结构
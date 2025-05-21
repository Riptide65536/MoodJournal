using MoodTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MoodTracker.Converters
{
    class EmotionConverter
    {
        // 将EmotionType转换为对应的字符串
        static public string ConvertToString(EmotionType emotion)
        {
            return emotion switch
            {
                EmotionType.Happy => "happy",
                EmotionType.Sad => "sad",
                EmotionType.Neutral => "neutral",
                EmotionType.Anxiety => "anxiety",
                EmotionType.Afraid => "afraid",
                EmotionType.Angry => "angry",
                EmotionType.Calm => "calm",
                EmotionType.Excited => "excited",
                EmotionType.Confused => "confused",
                _ => "",
            };
        }

        static public string ConvertToChinese(EmotionType emotion)
        {
            return emotion switch
            {
                EmotionType.Happy => "开心",
                EmotionType.Sad => "伤心",
                EmotionType.Neutral => "中性",
                EmotionType.Anxiety => "焦虑",
                EmotionType.Afraid => "害怕",
                EmotionType.Angry => "生气",
                EmotionType.Calm => "冷静",
                EmotionType.Excited => "激动",
                EmotionType.Confused => "疑惑",
                _ => "", // 默认返回一个表情
            };
        }

        static public string ConvertToEmojiPath(EmotionType emotion)
        {
            return emotion switch
            {
                EmotionType.Happy => "/Assets/Mood/happy.png",
                EmotionType.Sad => "/Assets/Mood/sad.png",
                EmotionType.Neutral => "/Assets/Mood/neutral.png",
                EmotionType.Anxiety => "/Assets/Mood/anxiety.png",
                EmotionType.Afraid => "/Assets/Mood/afraid.png",
                EmotionType.Angry => "/Assets/Mood/angry.png",
                EmotionType.Calm => "/Assets/Mood/calm.png",
                EmotionType.Excited => "/Assets/Mood/excited.png",
                EmotionType.Confused => "/Assets/Mood/confused.png",
                _ => "/Assets/Mood/neutral.png", // 默认返回一个表情
            };
        }

        static public SolidColorBrush ConvertToColor(EmotionType emotion)
        {
            return emotion switch
            {
                EmotionType.Happy => new SolidColorBrush(Color.FromRgb(255, 223, 186)),
                EmotionType.Sad => new SolidColorBrush(Color.FromRgb(186, 223, 255)),
                EmotionType.Neutral => new SolidColorBrush(Color.FromRgb(186, 255, 223)),
                EmotionType.Anxiety => new SolidColorBrush(Color.FromRgb(255, 186, 223)),
                EmotionType.Afraid => new SolidColorBrush(Color.FromRgb(223, 186, 255)),
                EmotionType.Angry => new SolidColorBrush(Color.FromRgb(255, 186, 186)),
                EmotionType.Calm => new SolidColorBrush(Color.FromRgb(186, 255, 186)),
                EmotionType.Excited => new SolidColorBrush(Color.FromRgb(255, 255, 186)),
                EmotionType.Confused => new SolidColorBrush(Color.FromRgb(223, 223, 223)),
                _ => new SolidColorBrush(Color.FromRgb(200, 200, 200)), // 默认颜色
            };
        }

        static public int ConvertToValue(EmotionType emotion)
        {
            return emotion switch
            {
                EmotionType.Happy => 90,
                EmotionType.Sad => 25,
                EmotionType.Neutral => 60,
                EmotionType.Anxiety => 30,
                EmotionType.Afraid => 20,
                EmotionType.Angry => 34,
                EmotionType.Calm => 80,
                EmotionType.Excited => 95,
                EmotionType.Confused => 50,
                _ => 60, // 默认值
            };
        }
    }
}

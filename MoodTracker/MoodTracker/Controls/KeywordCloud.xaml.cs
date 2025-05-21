using MoodTracker.Converters;
using MoodTracker.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoodTracker.Controls
{
    public partial class KeywordCloud : UserControl
    {
        public string currentUserId = "0";

        public KeywordCloud()
        {
            InitializeComponent();
            GenerateKeywordCloud();
        }

        private void GenerateKeywordCloud()
        {
            // 获取当前用户的所有情绪记录
            JournalService service = new();
            var records = service.GetRecordsByUserId(currentUserId);

            // 清点词云的关键词：情绪类型和标签
            var wordsDict = new Dictionary<string, int>();
            foreach(MoodRecord record in records)
            {
                // 情绪类型记录
                string emotion = EmotionConverter.ConvertToChinese(record.CurrentEmotion);
                if (wordsDict.ContainsKey(emotion))
                {
                    wordsDict[emotion]++;
                }
                else
                {
                    wordsDict[emotion] = 1;
                }

                // 对于每个标签记录
                foreach (var tag in record.Tags)
                {
                    string tagName = "#" + tag.Name;
                    if (wordsDict.ContainsKey(tagName))
                    {
                        wordsDict[tagName] += 2;
                    }
                    else
                    {
                        wordsDict[tagName] = 2;
                    }
                }
            }

            // 获取wordsDict中的极值
            int maxCount = 0;
            foreach (var e in wordsDict)
            {
                if (e.Value > maxCount)
                {
                    maxCount = e.Value;
                }
            }

            // 开始生成词云
            Random rand = new Random(DateTime.Now.Millisecond);
            // 只取前30个
            var topWords = wordsDict.OrderByDescending(x => x.Value).Take(30).ToList();

            foreach (var e in topWords)
            {
                int weight = (int)Math.Max(16, 32.0 * e.Value / maxCount);

                var tb = new TextBlock
                {
                    Text = e.Key,
                    Margin = new System.Windows.Thickness(5),
                    FontSize = weight,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(
                        (byte)rand.Next(100, 255),
                        (byte)rand.Next(100, 255),
                        (byte)rand.Next(100, 255))),
                };

                KeywordWrapPanel.Children.Add(tb);
            }
        }
    }
}

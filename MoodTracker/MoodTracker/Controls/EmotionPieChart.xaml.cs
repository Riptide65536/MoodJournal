using LiveCharts;
using LiveCharts.Wpf;
using MoodTracker.Converters;
using MoodTracker.Data;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoodTracker.Controls
{
    public partial class EmotionPieChart : UserControl
    {
        public string currentUserId = "0";
        public SeriesCollection? PieSeriesCollection { get; set; }

        public EmotionPieChart()
        {
            InitializeComponent();
            LoadChartData();
            DataContext = this;
        }

        private void LoadChartData()
        {
            // 从JournalServices中获取当前用户的情绪数据
            using var db = new ApplicationDbContext();
            var emotions = db.MoodRecords
                .Where(r => r.UserId == currentUserId)
                .Where(r => r.Datetime >= DateTime.Now.AddDays(-30))
                .Select(r => r.CurrentEmotion);

            // 分别统计每种情绪的数量
            var emotionCounts = new Dictionary<EmotionType, int>();
            foreach (var emotion in emotions)
            {
                if (emotionCounts.ContainsKey(emotion))
                {
                    emotionCounts[emotion]++;
                }
                else
                {
                    emotionCounts[emotion] = 1;
                }
            }

            // 创建饼图数据
            PieSeriesCollection = new SeriesCollection();
            foreach (var e in emotionCounts) {
                PieSeriesCollection.Add(new PieSeries
                {
                    Title = EmotionConverter.ConvertToChinese(e.Key),
                    Values = new ChartValues<double> { e.Value },
                    Fill = EmotionConverter.ConvertToColor(e.Key)
                });
            }
        }
    }
}

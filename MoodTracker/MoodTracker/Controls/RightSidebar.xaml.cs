using MoodTracker.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MoodTracker.Controls
{
    /// <summary>
    /// RightSidebar.xaml 的交互逻辑
    /// </summary>
    public partial class RightSidebar : UserControl
    {
        private HashSet<DateTime> _recordDates = new();
        public string currentUserId = "0";

        private static readonly string[] MoodQuotesList = new[]
        {
            "每天都是新的开始！",
            "保持微笑，心情会更好。",
            "阳光总在风雨后。",
            "相信自己，你可以的！",
            "用心感受生活的美好。"
        };

        public RightSidebar()
        {
            InitializeComponent();
            PunchCalendar.SelectedDatesChanged += PunchCalendar_SelectedDatesChanged;
            LoadRecordDates();
            PunchCalendar.Loaded += PunchCalendar_Loaded;
            SetRandomMoodQuote();
        }

        private void PunchCalendar_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            if (PunchCalendar.SelectedDate.HasValue)
            {
                // 处理日期选择变化
                DateTime selectedDate = PunchCalendar.SelectedDate.Value;
                // TODO: 根据选择的日期更新UI或执行其他操作
            }
        }

        private void LoadRecordDates()
        {
            // 假设有 JournalService.GetAllRecordDates() 返回 List<DateTime>
            var service = new JournalService();
            _recordDates = service.GetAllRecordDates(currentUserId).ToHashSet();
        }

        private void PunchCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            HighlightRecordDates();
        }

        private void PunchCalendar_DisplayDateChanged(object? sender, CalendarDateChangedEventArgs e)
        {
            HighlightRecordDates();
        }

        private void HighlightRecordDates()
        {
            var calendar = PunchCalendar;
            if (calendar == null) return;

            calendar.ApplyTemplate();
            var month = calendar.DisplayDate;

            foreach (var item in FindVisualChildren<CalendarDayButton>(calendar))
            {
                if (item.DataContext is DateTime date)
                {
                    if (_recordDates.Contains(date.Date))
                    {
                        item.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xD7, 0x00)); // 高亮色
                    }
                    else
                    {
                        item.ClearValue(Button.BackgroundProperty);
                    }
                }
            }
        }

        // 辅助方法：递归查找子元素
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void SetRandomMoodQuote()
        {
            var rand = new Random();
            MoodQuote.Text = MoodQuotesList[rand.Next(MoodQuotesList.Length)];
        }


        //心情小语实现
        private readonly SimpleAIChat _aiChat = new SimpleAIChat("sk-76716f1a8fca45a9bfe98c01b1a6c310");

        private async void RefreshMoodQuote_Click(object sender, RoutedEventArgs e)
        {
            MoodQuote.Text = "生成中...";
            try
            {
                string prompt = "请以轻松、温暖的语气生成一句中文心情语录，不超过30个字，不要出现AI相关内容，生成的语录要有创意,注意每次生成的语句不要和上一句生成的有很多相同（每句话里的元素不要重复）。";
                string quote = await _aiChat.GetChatResponse(prompt);
                MoodQuote.Text = quote.Trim();
            }
            catch (Exception ex)
            {
                MoodQuote.Text = "获取失败，请稍后重试。";
                Console.WriteLine(ex.Message);
            }
        }
    }
}

using MoodTracker.Data;
using System.Windows;
using System.Windows.Controls;

namespace MoodTracker.View
{
    public partial class RecordDetailPage : Page
    {
        public RecordDetailPage(MoodRecord record)
        {
            InitializeComponent();
            this.DataContext = record;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // 返回上一页（可根据你的 Frame 实例修改）
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }
    }
}

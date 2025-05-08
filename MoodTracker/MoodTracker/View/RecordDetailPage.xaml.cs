using System.Windows;
using System.Windows.Controls;
using MoodTracker.Models;

namespace MoodTracker.View
{
    public partial class RecordDetailPage : Page
    {
        public RecordDetailPage(RecordModel record)
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

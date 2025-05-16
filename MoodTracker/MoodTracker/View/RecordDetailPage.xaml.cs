using MoodTracker.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoodTracker.View
{
    public partial class RecordDetailPage : Page
    {
        private MoodRecord _record;
        public RecordDetailPage(MoodRecord record)
        {
            InitializeComponent();
            this.DataContext = record;
            _record = record;
            _record.PropertyChanged += Record_PropertyChanged;
        }

        private void Record_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender == null) return;
            // 调用你的自动保存逻辑
            JournalService service = new JournalService();
            service.UpdateMoodRecord(_record.RecordId, _record);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // 返回上一页（可根据你的 Frame 实例修改）
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }

        private void TitleTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                TitleTextBlock.Visibility = Visibility.Collapsed;
                TitleTextBox.Visibility = Visibility.Visible;
                TitleTextBox.Focus();
                TitleTextBox.SelectAll();
            }
        }

        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Visibility = Visibility.Collapsed;
            TitleTextBlock.Visibility = Visibility.Visible;
        }

        private void TitleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TitleTextBox.Visibility = Visibility.Collapsed;
                TitleTextBlock.Visibility = Visibility.Visible;
            }
        }

        

    }
}

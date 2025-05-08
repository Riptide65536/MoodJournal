using MoodTracker.View;
using MoodTracker.Controls;
using System.Windows;
using System.Windows.Controls;
using MoodTracker.Models;


namespace MoodTracker.View
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void OnRecordSelected(object? sender, RecordModel model)
        {
            var detailPage = new RecordDetailPage(model);
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentFrame.Navigate(detailPage);
        }
    }


}

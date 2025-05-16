using System.Windows;
using System.Windows.Input;
using MoodTracker.ViewModels;
using MoodTracker.View;
using System.Windows.Controls;
using MoodTracker.Controls;
using System.Globalization;
using System.Windows.Data;
using MoodTracker.Data;

namespace MoodTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            // 默认导航到首页
            MainContentFrame.Navigate(new HomePage());
        }

        //页面切换逻辑
        public void NavigateTo(Page page)
        {
            MainContentFrame.Navigate(page);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SideNav_NavigationRequested(object sender, RoutedEventArgs e)
        {
            if (sender is SideNav sideNav)
            {
                if (sideNav.HomeButton.IsChecked == true)
                {
                    MainContentFrame.Navigate(new HomePage());
                }
                else if (sideNav.AnalyticsButton.IsChecked == true)
                {
                    MainContentFrame.Navigate(new DataAnalysisPage());
                }
                else if (sideNav.AIButton.IsChecked == true)
                {
                    MainContentFrame.Navigate(new ChatWindow());
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // 创建一个新的记录
            MoodRecord moodRecord = new MoodRecord();

            // 提醒RecordList组件更新
            var homePage = MainContentFrame.Content as HomePage;
            if (homePage != null)
            {
                // 假设HomePage里有public属性RecordList RecordListControl { get; }
                homePage.RecordListControl.AddNewRecord(moodRecord);
            }

            // 导航至新开的页面
            var detailPage = new RecordDetailPage(moodRecord);

            ((MainWindow)Application.Current.MainWindow).MainContentFrame.Navigate(detailPage);
        }
    }
}

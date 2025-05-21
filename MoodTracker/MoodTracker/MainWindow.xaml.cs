using System.Windows;
using System.Windows.Input;
using MoodTracker.ViewModels;
using MoodTracker.View;
using System.Windows.Controls;
using MoodTracker.Controls;
using System.Globalization;
using System.Windows.Data;
using MoodTracker.Data;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Linq;
using System.Windows.Threading;

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
            // 监听主窗口任何点击事件
            this.PreviewMouseDown += MainWindow_PreviewMouseDown;
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
                else if (sideNav.MemoryButton.IsChecked == true)
                {
                    MoodRecord? record = new JournalService().GetRandomRecordByUserId("0");
                    if (record == null) return;
                    var detailPage = new RecordDetailPage(record);
                    MainContentFrame.Navigate(detailPage);
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


        //搜索框相关事件（5.20）
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.UpdateSearchResults();
                viewModel.IsSearchOpen = true;
                SearchPopup.IsOpen = true; // 确保弹窗打开
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // 延迟检查是否真的需要关闭弹窗
            Dispatcher.BeginInvoke(new Action(() =>
            {
                // 检查焦点是否真的离开了搜索区域
                if (!SearchPopup.IsMouseOver && !SearchBox.IsMouseOver)
                {
                    if (DataContext is MainViewModel viewModel)
                    {
                        viewModel.IsSearchOpen = false;
                        SearchPopup.IsOpen = false;
                    }
                }
            }), DispatcherPriority.Background);
        }

        private void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is MoodRecord selectedRecord)
            {
                // 导航到选中的记录详情页
                var detailPage = new RecordDetailPage(selectedRecord);
                MainContentFrame.Navigate(detailPage);

                // 关闭下拉
                SearchPopup.IsOpen = false;

                // 清除选中，避免再次点击无效
                ((ListBox)sender).SelectedItem = null;
            }
        }

        private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // 检查点击是否发生在搜索框或搜索列表内
            if (!IsClickInside(SearchBox, e) && !IsClickInside(SearchPopup, e))
            {
                // 延迟关闭，以便能够点击搜索结果
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!SearchPopup.IsMouseOver && !SearchBox.IsMouseOver)
                    {
                        if (DataContext is MainViewModel vm)
                        {
                            vm.IsSearchOpen = false;
                            SearchPopup.IsOpen = false;
                        }
                    }
                }), DispatcherPriority.Background);
            }
        }
        private bool IsClickInside(FrameworkElement element, MouseButtonEventArgs e)
        {
            if (element == null) return false;

            var clickedElement = e.OriginalSource as DependencyObject;
            return element.IsAncestorOf(clickedElement);
        }

        private void SearchBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 手动设置焦点 & 打开弹窗
            SearchBox.Focus();
            e.Handled = true; // 防止事件冒泡

            if (DataContext is MainViewModel viewModel)
            {
                viewModel.UpdateSearchResults();
                viewModel.IsSearchOpen = true;
                SearchPopup.IsOpen = true; // 确保弹窗打开
            }
        }

    }
}

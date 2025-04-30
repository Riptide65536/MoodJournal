using MoodTracker.View;
using System.Windows;
using System.Windows.Controls;

namespace MoodTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //页面初始加载为HomePage
            MainContentFrame.Navigate(new HomePage());
        }

        //页面切换逻辑
        public void NavigateTo(Page page)
        {
            MainContentFrame.Navigate(page);
        }

        // 拖动窗口
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        // 最小化按钮事件
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // 关闭按钮事件
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

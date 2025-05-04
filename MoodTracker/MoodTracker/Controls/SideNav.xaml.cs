using System.Windows;
using System.Windows.Controls;
using MoodTracker.View;

using System.Windows.Media.Animation;


namespace MoodTracker.Controls
{
    /// <summary>
    /// SideNav.xaml 的交互逻辑
    /// </summary>
    public partial class SideNav : UserControl
    {
        public SideNav()
        {
            InitializeComponent();

            // 初始宽度设置
            this.Width = IsExpanded ? ExpandedWidth : CollapsedWidth;
        }

        private const double ExpandedWidth = 200;
        private const double CollapsedWidth = 85;

        // 是否展开的依赖属性
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(SideNav),
                new PropertyMetadata(true, OnIsExpandedChanged));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        // 当 IsExpanded 变化时触发
        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SideNav nav)
            {
                nav.UpdateWidthAnimation((bool)e.NewValue);
            }
        }

        private void UpdateWidthAnimation(bool expanded)
        {
            double toWidth = expanded ? ExpandedWidth : CollapsedWidth;
            var anim = new DoubleAnimation
            {
                To = toWidth,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };
            this.BeginAnimation(WidthProperty, anim);
        }

        //清除按钮选中状态
        private void ResetSelection()
        {
            MenuButton.IsChecked = false;
            HomeButton.IsChecked = false;
            AnalyticsButton.IsChecked = false;
            //待添加（别的功能按钮）
        }


        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
            MenuButton.IsChecked = true;
            IsExpanded = !IsExpanded;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
            HomeButton.IsChecked = true;
            // TODO: 导航到首页
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateTo(new HomePage());
            }
        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
            AnalyticsButton.IsChecked = true;
            // TODO: 导航到数据分析页
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateTo(new DataAnalysisPage());
            }
        }
    }
}

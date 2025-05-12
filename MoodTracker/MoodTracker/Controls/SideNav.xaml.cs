using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MoodTracker.ViewModels;
using System.Windows.Media.Animation;
using MoodTracker.View;

namespace MoodTracker.Controls
{
    /// <summary>
    /// SideNav.xaml 的交互逻辑
    /// </summary>
    public partial class SideNav : UserControl
    {
        public static readonly RoutedEvent NavigationRequestedEvent =
            EventManager.RegisterRoutedEvent("NavigationRequested", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SideNav));

        public event RoutedEventHandler NavigationRequested
        {
            add { AddHandler(NavigationRequestedEvent, value); }
            remove { RemoveHandler(NavigationRequestedEvent, value); }
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

        public SideNav()
        {
            InitializeComponent();
            //DataContext = new MainViewModel();

            // 初始宽度设置
            this.Width = IsExpanded ? ExpandedWidth : CollapsedWidth;
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
            AIButton.IsChecked = false;
            //待添加（别的功能按钮）
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
            IsExpanded = !IsExpanded;
            if (sender is ToggleButton button)
            {
                button.IsChecked = IsExpanded;
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
            if (sender is ToggleButton homeButton)
            {
                var analyticsButton = this.FindName("AnalyticsButton") as ToggleButton;
                if (analyticsButton != null)
                {
                    homeButton.IsChecked = true;
                    analyticsButton.IsChecked = false;
                    RaiseEvent(new RoutedEventArgs(NavigationRequestedEvent, "Home"));
                }
            }
        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
            if (sender is ToggleButton analyticsButton)
            {
                var homeButton = this.FindName("HomeButton") as ToggleButton;
                if (homeButton != null)
                {
                    homeButton.IsChecked = false;
                    analyticsButton.IsChecked = true;
                    RaiseEvent(new RoutedEventArgs(NavigationRequestedEvent, "Analytics"));
                }
            }
        }

        private void AIButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
            if (sender is ToggleButton aiButton)
            {
                var homeButton = this.FindName("HomeButton") as ToggleButton;
                var analyticsButton = this.FindName("AnalyticsButton") as ToggleButton;

                if (homeButton != null) homeButton.IsChecked = false;
                if (analyticsButton != null) analyticsButton.IsChecked = false;
                aiButton.IsChecked = true;

                RaiseEvent(new RoutedEventArgs(NavigationRequestedEvent, "AI"));
            }
        }
    }
}

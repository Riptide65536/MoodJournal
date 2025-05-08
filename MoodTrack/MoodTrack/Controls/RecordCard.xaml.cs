using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoodTracker.Controls
{
    /// <summary>
    /// RecordCard.xaml 的交互逻辑
    /// </summary>
    public partial class RecordCard : UserControl
    {
        public RecordCard()
        {
            InitializeComponent();
        }

        // 自定义依赖属性：Date
        public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register("Date", typeof(DateTime),
            typeof(RecordCard), new PropertyMetadata(DateTime.Now.Date));

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RecordCard;
            if (control != null)
            {
                control.DateTextBlock.Text = (string)e.NewValue;
            }
        }

        // 自定义依赖属性：Mood
        public static readonly DependencyProperty MoodProperty =
            DependencyProperty.Register("Mood", typeof(string), typeof(RecordCard), new PropertyMetadata(string.Empty));

        public string Mood
        {
            get { return (string)GetValue(MoodProperty); }
            set { SetValue(MoodProperty, value); }
        }
        private static void OnMoodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RecordCard;
            if (control != null)
            {
                // 更新显示
                control.MoodTextBlock.Text = (string)e.NewValue;
            }
        }

        // 自定义点击事件，供外部绑定
        public event RoutedEventHandler CardClicked;

        private ScaleTransform _scaleTransform = new ScaleTransform(1.0, 1.0);

        private void CardBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            CardBorder.RenderTransformOrigin = new Point(0.5, 0.5);
            CardBorder.RenderTransform = _scaleTransform;

            AnimateScale(_scaleTransform.ScaleX, 1.05);
        }

        private void CardBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateScale(_scaleTransform.ScaleX, 1.0);
        }

        private void CardBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AnimateScale(_scaleTransform.ScaleX, 0.95);
        }

        private void CardBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AnimateScale(_scaleTransform.ScaleX, 1.05);
            CardClicked?.Invoke(this, new RoutedEventArgs());
        }

        // 缩放动画封装
        private void AnimateScale(double from, double to)
        {
            var anim = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(150))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            _scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            _scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }
    }
}


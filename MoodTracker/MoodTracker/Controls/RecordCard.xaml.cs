using MoodTracker.Converters;
using MoodTracker.Data;
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
            this.Date = DateTime.Now;
            this.Title = string.Empty;
            this.Record = new();
            InitializeComponent();
        }

        public RecordCard(DateTime dateTime, string title, MoodRecord record)
        {
            this.Date = dateTime;
            this.Title = title;
            this.Record = record;
            InitializeComponent();
        }

        // 重写依赖属性Date和Mood

        public DateTime date;
        public DateTime Date
        {
            get => date;
            set { date = value; }
        }

        public string Format_date{get => date.ToString("f");}

        public string? title;
        public string Title
        {
            get => (title == null || title == string.Empty ? "无标题" : title);
            set { title = value; }
        }

        // 对应的emoji图片
        public string ImagePath{get => EmotionConverter.ConvertToEmojiPath(Record.CurrentEmotion);}

        // 对应的record属性
        public MoodRecord Record { get; set; }

        // 自定义点击事件，供外部绑定
        public event RoutedEventHandler? CardClicked;//卡片点击事件

        private ScaleTransform _scaleTransform = new ScaleTransform(1.0, 1.0);

        public event RoutedEventHandler? OptionsButtonClicked;//右上角事件点击（删除当前卡片）

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
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            AnimateScale(_scaleTransform.ScaleX, 1.05);
            OptionsButtonClicked?.Invoke(this, e);
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


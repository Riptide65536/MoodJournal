using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoodTracker.Controls
{
    /// <summary>
    /// MusicDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class MusicDisplay : UserControl, INotifyPropertyChanged
    {
        private bool isPlaying = false;  // 当前播放状态
        private double musicProgress = 0;  // 当前进度
        private System.Windows.Threading.DispatcherTimer progressTimer;

        public MusicDisplay()
        {
            InitializeComponent();
            DataContext = this;

            DateTime today = DateTime.Today;
            DayOfWeekText.Text = today.ToString("dddd", new CultureInfo("zh-CN"));
            DateText.Text = today.ToString("yyyy-MM-dd"); // 2025-04-28
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // MusicProgress属性，用于绑定进度条
        public double MusicProgress
        {
            get => musicProgress;
            set
            {
                musicProgress = value;
                OnPropertyChanged(nameof(MusicProgress));
            }
        }

        // 播放/暂停按钮点击事件
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = !isPlaying;

            // 切换播放/暂停图标
            if (isPlaying)
            {
                PlayPauseIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/pause.png"));
                StartMusicProgressUpdate();
            }
            else
            {
                PlayPauseIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/play.png"));
            }
        }

        private void StartMusicProgressUpdate()
        {
            // 假设每秒更新进度
            if (progressTimer == null)
            {
                progressTimer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                progressTimer.Tick += (s, e) =>
                {
                    if (musicProgress < 100)
                    {
                        MusicProgress += 10;
                    }
                    else
                    {
                        progressTimer.Stop();  // 播放完成
                    }
                };
            }
            progressTimer.Start();
        }

        // 上一首、下一首等按钮事件
        private void PreviousButton_Click(object sender, RoutedEventArgs e) { }
        private void NextButton_Click(object sender, RoutedEventArgs e) { }
    }
}

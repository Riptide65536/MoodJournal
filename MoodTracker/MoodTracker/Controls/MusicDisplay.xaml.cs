using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MoodTracker.Controls
{
    public partial class MusicDisplay : UserControl, INotifyPropertyChanged
    {
        private bool isPlaying = false;
        private double musicProgress = 0;
        private string progressText = "0:00 / 0:00";
        private System.Windows.Threading.DispatcherTimer progressTimer;

        // 当前播放的歌曲信息
        private MusicItem currentSong = new MusicItem
        {
            Title = "XX歌曲",
            Artist = "XX歌手",
            CoverImage = "pack://application:,,,/Assets/MoodTracker.png",
            Duration = 180 // 3分钟
        };

        public MusicDisplay()
        {
            InitializeComponent();
            DataContext = this;

            // 初始化日期显示
            DateTime today = DateTime.Today;
            DayOfWeekText.Text = today.ToString("dddd", new CultureInfo("zh-CN"));
            DateText.Text = today.ToString("yyyy-MM-dd");

            // 初始化歌曲信息
            UpdateSongInfo();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double MusicProgress
        {
            get => musicProgress;
            set
            {
                musicProgress = value;
                OnPropertyChanged(nameof(MusicProgress));
                UpdateProgressText();
            }
        }

        public string ProgressText
        {
            get => progressText;
            set
            {
                progressText = value;
                OnPropertyChanged(nameof(ProgressText));
            }
        }

        private void UpdateSongInfo()
        {
            SongNameText.Text = currentSong.Title;
            ArtistNameText.Text = currentSong.Artist;
            CoverImage.Source = new BitmapImage(new Uri(currentSong.CoverImage));
            MusicProgress = 0;
            UpdateProgressText();
        }

        private void UpdateProgressText()
        {
            int currentSeconds = (int)(currentSong.Duration * (MusicProgress / 100));
            TimeSpan currentTime = TimeSpan.FromSeconds(currentSeconds);
            TimeSpan totalTime = TimeSpan.FromSeconds(currentSong.Duration);

            ProgressText = $"{currentTime:mm\\:ss} / {totalTime:mm\\:ss}";
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = !isPlaying;

            if (isPlaying)
            {
                PlayPauseIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/pause.png"));
                StartMusicProgressUpdate();
            }
            else
            {
                PlayPauseIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/play.png"));
                StopMusicProgressUpdate();
            }
        }

        private void StartMusicProgressUpdate()
        {
            if (progressTimer == null)
            {
                progressTimer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                progressTimer.Tick += (s, e) =>
                {
                    if (MusicProgress < 100)
                    {
                        // 计算每秒增加的进度百分比
                        double increment = 100.0 / currentSong.Duration;
                        MusicProgress += increment;
                    }
                    else
                    {
                        // 播放完成
                        StopMusicProgressUpdate();
                        MusicProgress = 0;
                        isPlaying = false;
                        PlayPauseIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/play.png"));
                    }
                };
            }
            progressTimer.Start();
        }

        private void StopMusicProgressUpdate()
        {
            progressTimer?.Stop();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里可以替换为实际的上一首逻辑
            currentSong = new MusicItem
            {
                Title = "上一首歌曲",
                Artist = "上一首艺术家",
                CoverImage = "pack://application:,,,/Assets/MoodTracker.png",
                Duration = 210 // 3分30秒
            };

            UpdateSongInfo();
            if (isPlaying)
            {
                MusicProgress = 0;
                StartMusicProgressUpdate();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里可以替换为实际的下一首逻辑
            currentSong = new MusicItem
            {
                Title = "下一首歌曲",
                Artist = "下一首艺术家",
                CoverImage = "pack://application:,,,/Assets/MoodTracker.png",
                Duration = 150 // 2分30秒
            };

            UpdateSongInfo();
            if (isPlaying)
            {
                MusicProgress = 0;
                StartMusicProgressUpdate();
            }
        }
    }

    // 简单的歌曲信息类
    public class MusicItem
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string CoverImage { get; set; }
        public int Duration { get; set; } // 歌曲时长，单位秒
    }
}
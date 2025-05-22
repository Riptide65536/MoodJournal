using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using NAudio.Wave;
using System.IO;
namespace MoodTracker.Controls
{
    public partial class MusicDisplay : UserControl, INotifyPropertyChanged
    {
        private bool isPlaying = false;
        private double musicProgress = 0;
        private string progressText = "0:00 / 0:00";
        private System.Windows.Threading.DispatcherTimer progressTimer;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFileReader;

        private readonly string[] songPaths = new string[]
        {
              "Assets/跳楼机_时柏尘.wav",
              "Assets/稻香_周杰伦.wav",
              "Assets/搀扶_马健涛.wav"
        };
        private int currentSongIndex = 0;

        // 当前播放的歌曲信息
        private MusicItem currentSong = new MusicItem
        {
            Title = "未加载歌曲",
            Artist = "未知艺术家",
            CoverImage = "pack://application:,,,/Assets/MoodTracker.png",
            Duration = 0
        };

        public MusicDisplay()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += MusicDisplay_Loaded;
            Unloaded += MusicDisplay_Unloaded;
        }

        private void MusicDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化日期显示
            DateTime today = DateTime.Today;
            DayOfWeekText.Text = today.ToString("dddd", new CultureInfo("zh-CN"));
            DateText.Text = today.ToString("yyyy-MM-dd");

            // 初始化第一首歌信息
            if (songPaths.Length > 0)
            {
                UpdateCurrentSongInfo();
            }
        }

        private void MusicDisplay_Unloaded(object sender, RoutedEventArgs e)
        {
            CleanupResources();
        }

        private void CleanupResources()
        {
            try
            {
                outputDevice?.Stop();
                outputDevice?.Dispose();
                audioFileReader?.Dispose();
                progressTimer?.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"资源释放异常: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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

        private void UpdateCurrentSongInfo()
        {
            // 从文件名解析歌曲名和歌手（格式：歌曲名_歌手.wav）
            string fileName = Path.GetFileNameWithoutExtension(songPaths[currentSongIndex]);
            string[] parts = fileName.Split(new[] { '_' }, 2); // 按第一个下划线分割

            currentSong.Title = parts.Length > 0 ? parts[0] : "未知歌曲";
            currentSong.Artist = parts.Length > 1 ? parts[1] : "未知艺术家";
            currentSong.Duration = audioFileReader?.TotalTime.TotalSeconds ?? 0;

            // 更新UI
            SongNameText.Text = currentSong.Title;
            ArtistNameText.Text = currentSong.Artist;
            CoverImage.Source = new BitmapImage(new Uri(currentSong.CoverImage));
            MusicProgress = 0;
            UpdateProgressText();
        }

        private void UpdateProgressText()
        {
            if (audioFileReader == null) return;

            TimeSpan currentTime = audioFileReader.CurrentTime;
            TimeSpan totalTime = audioFileReader.TotalTime;
            ProgressText = $"{currentTime:mm\\:ss} / {totalTime:mm\\:ss}";
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += (s, args) =>
                {
                    if (isPlaying && args.Exception == null)
                    {
                        Dispatcher.Invoke(() => NextButton_Click(null, null));
                    }
                };
            }

            if (isPlaying)
            {
                PauseMusic();
            }
            else
            {
                PlayMusic();
            }
        }

        private void PlayMusic()
        {
            if (audioFileReader == null)
            {
                audioFileReader = new AudioFileReader(songPaths[currentSongIndex]);
            }

            // Stop any previously playing music before starting a new one
            outputDevice?.Stop();
            outputDevice?.Init(audioFileReader);
            outputDevice?.Play();

            // Update UI state
            PlayPauseIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/pause.png"));
            StartMusicProgressUpdate();
            isPlaying = true;
        }

        private void PauseMusic()
        {
            outputDevice.Pause();
            PlayPauseIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/play.png"));
            StopMusicProgressUpdate();
            isPlaying = false;
        }

        private void StartMusicProgressUpdate()
        {
            progressTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            progressTimer.Tick += (s, e) =>
            {
                if (audioFileReader != null)
                {
                    MusicProgress = (audioFileReader.CurrentTime.TotalSeconds / audioFileReader.TotalTime.TotalSeconds) * 100;
                }
            };
            progressTimer.Start();
        }

        private void StopMusicProgressUpdate()
        {
            progressTimer?.Stop();
            progressTimer = null;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            currentSongIndex = (currentSongIndex - 1 + songPaths.Length) % songPaths.Length;
            SwitchSong();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentSongIndex = (currentSongIndex + 1) % songPaths.Length;
            SwitchSong();
        }

        private void SwitchSong()
        {
            try
            {
                // Stop the current song
                outputDevice?.Stop();

                // Dispose of the previous audio file reader
                audioFileReader?.Dispose();
                audioFileReader = null;

                // Load the new song
                if (isPlaying)
                {
                    PlayMusic(); // If the song is playing, play the next one immediately
                }
                else
                {
                    // If paused, just load the next song without playing
                    audioFileReader = new AudioFileReader(songPaths[currentSongIndex]);
                    UpdateCurrentSongInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"切换歌曲失败: {ex.Message}");
            }
        }

        public class MusicItem
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public string CoverImage { get; set; }
            public double Duration { get; set; }
        }
    }
}
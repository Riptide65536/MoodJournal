using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoodTracker.Controls
{
    public partial class KeywordCloud : UserControl
    {
        public KeywordCloud()
        {
            InitializeComponent();
            GenerateKeywordCloud();
        }

        private void GenerateKeywordCloud()
        {
            string[] keywords = { "轻松", "放松", "热情", "悲伤", "冥想", "能量", "梦幻", "自然", "孤独", "幸福" };
            Random rand = new Random();

            foreach (var word in keywords)
            {
                var tb = new TextBlock
                {
                    Text = word,
                    Margin = new System.Windows.Thickness(5),
                    FontSize = rand.Next(16, 30),
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(
                        (byte)rand.Next(100, 255),
                        (byte)rand.Next(100, 255),
                        (byte)rand.Next(100, 255)))
                };

                KeywordWrapPanel.Children.Add(tb);
            }
        }
    }
}

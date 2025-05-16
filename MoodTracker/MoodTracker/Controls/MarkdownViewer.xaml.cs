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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoodTracker.Controls
{
    
    public partial class MarkdownViewer : UserControl
    {
        public static readonly DependencyProperty MarkdownProperty =
           DependencyProperty.Register("Markdown", typeof(string), typeof(MarkdownViewer),
               new PropertyMetadata("", OnMarkdownChanged));

        public string Markdown
        {
            get { return (string)GetValue(MarkdownProperty); }
            set { SetValue(MarkdownProperty, value); }
        }
        public MarkdownViewer()
        {
            InitializeComponent();
        }

        private static void OnMarkdownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MarkdownViewer viewer)
            {
                viewer.RenderMarkdown(e.NewValue as string ?? "");
            }
        }

        private void RenderMarkdown(string markdown)
        {
            MarkdownTextBlock.Inlines.Clear();

            // 简单的Markdown解析实现
            var lines = markdown.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            bool inCodeBlock = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("```"))
                {
                    inCodeBlock = !inCodeBlock;
                    continue;
                }

                if (inCodeBlock)
                {
                    AddFormattedText(line, Brushes.Black, Brushes.LightGray, FontFamily, 14, FontWeights.Normal);
                }
                else if (line.StartsWith("# "))
                {
                    AddFormattedText(line.Substring(2) + "\n", Brushes.DarkBlue, null, new FontFamily("Arial"), 18, FontWeights.Bold);
                }
                else if (line.StartsWith("## "))
                {
                    AddFormattedText(line.Substring(3) + "\n", Brushes.DarkBlue, null, new FontFamily("Arial"), 16, FontWeights.Bold);
                }
                else if (line.StartsWith("### "))
                {
                    AddFormattedText(line.Substring(4) + "\n", Brushes.DarkBlue, null, new FontFamily("Arial"), 14, FontWeights.Bold);
                }
                else if (line.StartsWith("* ") || line.StartsWith("- "))
                {
                    AddFormattedText("• " + line.Substring(2) + "\n", Brushes.Black, null, FontFamily, 14, FontWeights.Normal);
                }
                else if (line.StartsWith("> "))
                {
                    AddFormattedText(line + "\n", Brushes.Gray, null, FontFamily, 14, FontWeights.Normal);
                }
                else
                {
                    AddFormattedText(line + "\n", Brushes.Black, null, FontFamily, 14, FontWeights.Normal);
                }
            }
        }

        private void AddFormattedText(string text, Brush foreground, Brush background,
                                    FontFamily fontFamily, double fontSize, FontWeight fontWeight)
        {
            var run = new Run(text)
            {
                Foreground = foreground,
                Background = background ?? Brushes.Transparent,
                FontFamily = fontFamily,
                FontSize = fontSize,
                FontWeight = fontWeight
            };

            MarkdownTextBlock.Inlines.Add(run);
        }
    }
}

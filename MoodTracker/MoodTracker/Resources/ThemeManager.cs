using System.Windows;
using System.Linq;

namespace MoodTracker.Resources
{
    public static class ThemeManager
    {
        public static void ApplyTheme(bool isDarkTheme)
        {
            var app = Application.Current;
            var resources = app.Resources;
            var mergedDictionaries = resources.MergedDictionaries;

            // 移除当前主题
            var currentTheme = mergedDictionaries.FirstOrDefault(d =>
                d.Source != null && (d.Source.OriginalString.Contains("LightTheme.xaml") || d.Source.OriginalString.Contains("DarkTheme.xaml")));

            if (currentTheme != null)
            {
                mergedDictionaries.Remove(currentTheme);
            }

            // 添加新主题
            var themeUri = isDarkTheme
                ? "Resources/DarkTheme.xaml"
                : "Resources/LightTheme.xaml";

            mergedDictionaries.Insert(0, new ResourceDictionary { Source = new System.Uri(themeUri, System.UriKind.Relative) });
        }
    }
} 
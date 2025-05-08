using System.Windows;

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
            if (mergedDictionaries.Count > 1)
            {
                mergedDictionaries.RemoveAt(1);
            }

            // 添加新主题
            var themeUri = !isDarkTheme
                ? "Resources/DarkTheme.xaml"
                : "Resources/LightTheme.xaml";

            mergedDictionaries.Add(new ResourceDictionary { Source = new System.Uri(themeUri, System.UriKind.Relative) });
        }
    }
} 
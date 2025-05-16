using System;
using System.Globalization;
using System.Windows.Data;

namespace MoodTracker.Data
{
    public class TitleDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var title = value as string;
            return string.IsNullOrWhiteSpace(title) ? "双击以编辑标题" : title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 只做显示，不做反向绑定
            return value;
        }
    }
}


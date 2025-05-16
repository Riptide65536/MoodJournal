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
            return string.IsNullOrWhiteSpace(title) ? "˫���Ա༭����" : title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ֻ����ʾ�����������
            return value;
        }
    }
}


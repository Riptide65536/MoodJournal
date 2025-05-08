using System;
using System.Windows;
using System.Windows.Controls;

namespace MoodTracker.Controls
{
    /// <summary>
    /// RightSidebar.xaml 的交互逻辑
    /// </summary>
    public partial class RightSidebar : UserControl
    {
        public RightSidebar()
        {
            InitializeComponent();
            PunchCalendar.SelectedDatesChanged += PunchCalendar_SelectedDatesChanged;
        }

        private void PunchCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PunchCalendar.SelectedDate.HasValue)
            {
                // 处理日期选择变化
                DateTime selectedDate = PunchCalendar.SelectedDate.Value;
                // TODO: 根据选择的日期更新UI或执行其他操作
            }
        }
    }
}

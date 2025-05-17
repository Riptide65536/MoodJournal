using MoodTracker.View;
using MoodTracker.Controls;
using System.Windows;
using System.Windows.Controls;
using MoodTracker.Data;
using System.Collections.ObjectModel;


namespace MoodTracker.View
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        public ObservableCollection<MoodRecord> UserRecords { get; set; }
        public RecordList RecordListControl => this.RecordListControl_;

        public string currentUserId = "0";

        public HomePage()
        {
            InitializeComponent();
            UserRecords = new ObservableCollection<MoodRecord>();
            this.DataContext = this;

            // 在这里提醒更新即可
            RecordListControl.currentUserId = currentUserId;
            RecordListControl.RefreshRecords();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshRecords();
        }


        public void RefreshRecords()
        {
            RecordListControl.RefreshRecords();
        }
    }
}

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

        public HomePage()
        {
            InitializeComponent();
            UserRecords = new ObservableCollection<MoodRecord>();
            this.DataContext = this;

            LoadUserRecords("guest"); // 替换为实际的用户ID
        }

        private void LoadUserRecords(string userId)
        {
            var journalService = new JournalService();
            var records = journalService.GetRecordsByUserId(userId);

            UserRecords.Clear();
            foreach (var record in records)
            {
                UserRecords.Add(record);
            }
        }

        private void OnRecordSelected(object? sender, MoodRecord record)
        {
            var detailPage = new RecordDetailPage(record);
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentFrame.Navigate(detailPage);
        }
    }


}

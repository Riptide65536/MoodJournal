using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// RecordList.xaml 的交互逻辑
    /// </summary>
    public partial class RecordList : UserControl
    {
        public ObservableCollection<RecordCard> RecordCards { get; set; }

        public RecordList()
        {
            InitializeComponent();
            RecordCards = new ObservableCollection<RecordCard>();
            this.DataContext = this;
            LoadInitialRecords();
        }

        // 加载初始数据
        private void LoadInitialRecords()
        {
            var card = new RecordCard
            {
                Mood = "今天心情超好！",
                Date = DateTime.Parse("2025-04-28")
            };
            RecordStackPanel.Children.Add(card);
            for (int i = 0; i < 20; i++) // 初始加载20个记录
            {
                var recordCard = new RecordCard
                {
                    Date = DateTime.Now.AddDays(-i),
                    Mood = "开心",
                };

                recordCard.CardClicked += RecordCard_Clicked;

                RecordCards.Add(recordCard);
                RecordStackPanel.Children.Add(recordCard); // 添加到界面
            }
        }

        // 每次滚动到底部时加载更多记录
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight)
            {
                LoadMoreRecords();
            }
        }

        // 加载更多记录
        private void LoadMoreRecords()
        {
            for (int i = 0; i < 20; i++)
            {
                var recordCard = new RecordCard
                {
                    Date = DateTime.Now.AddDays(-RecordCards.Count),
                    Mood = "放松",
                };

                recordCard.CardClicked += RecordCard_Clicked;

                RecordCards.Add(recordCard);
                RecordStackPanel.Children.Add(recordCard); // 添加到界面
            }
        }

        // 处理点击事件
        private void RecordCard_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("卡片被点击了!");
        }

        private void RecordCard2_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

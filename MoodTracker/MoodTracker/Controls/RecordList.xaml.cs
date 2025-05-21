using MoodTracker.Data;
using MoodTracker.View;
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
        private List<MoodRecord> allRecords = []; // 存储所有记录
        private int currentPage = 0; // 当前页数
        private const int PageSize = 10; // 每页加载的记录数

        // 这个id之后要用！
        public string currentUserId { get; set; } = "0";

        public RecordList()
        {
            InitializeComponent();
            RecordCards = [];
            this.DataContext = this;
            RefreshRecords();
        }

        // 加载初始数据
        public void RefreshRecords()
        {
            RecordCards = [];   // 清空当前所有卡片
            RecordStackPanel.Children.Clear();
            currentPage = 0;

            JournalService journalService = new();
            allRecords = journalService.GetRecordsByUserId(currentUserId); // 获取所有记录
            LoadMoreRecords(); // 加载第一页
        }

        private void Loaded_list(object sender, RoutedEventArgs e) { RefreshRecords(); }

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
            if (allRecords == null || currentPage * PageSize >= allRecords.Count)
            {
                return; // 没有更多记录
            }

            var recordsToLoad = allRecords
                .Skip(currentPage * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var rec in recordsToLoad)
            {
                var card = new RecordCard
                {
                    Title = rec.Title,
                    Date = rec.Datetime,
                    Record = rec // 将记录数据存储在 Tag 属性中
                };

                card.CardClicked += RecordCard_Clicked;
                card.OptionsButtonClicked += RecordCard_OptionsButtonClicked;
                RecordStackPanel.Children.Add(card); // 添加到界面
            }

            currentPage++; // 增加页数
        }

        // 处理点击事件
        private void RecordCard_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is RecordCard card && card.Record is MoodRecord record)
            {
                var detailPage = new RecordDetailPage(record);

                // 找到 MainWindow 里的 Frame 并导航
                ((MainWindow)Application.Current.MainWindow).MainContentFrame.Navigate(detailPage);
            }
        }

        //删除
        private void RecordCard_OptionsButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is RecordCard card && card.Record is MoodRecord record)
            {
                // 创建上下文菜单
                ContextMenu menu = new ContextMenu();

                // 删除菜单项
                MenuItem deleteItem = new MenuItem { Header = "删除该记录" };
                deleteItem.Click += (s, args) =>
                {
                    // 从 UI 中移除
                    RecordStackPanel.Children.Remove(card);

                    JournalService journalService = new();
                    journalService.DeleteMoodRecord(record.RecordId);

                    // 更新初始数据
                    RefreshRecords();

                };

                menu.Items.Add(deleteItem);

                // 弹出菜单位置绑定 OptionsButton
                if (e.OriginalSource is FrameworkElement source)
                {
                    menu.PlacementTarget = source;
                    menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                    menu.IsOpen = true;
                }
            }
        }

        //添加
        public void AddNewRecord(MoodRecord newRecord)
        {
            if (newRecord == null || currentUserId == null) return;

            // 处理record的用户信息
            newRecord.UserId = currentUserId;

            // 将record写入数据库
            JournalService journalService = new();
            journalService.AddRecordToExistingUser(currentUserId, newRecord);

            // 直接更新一遍卡片列表即可
            RefreshRecords();
        }
    }
}

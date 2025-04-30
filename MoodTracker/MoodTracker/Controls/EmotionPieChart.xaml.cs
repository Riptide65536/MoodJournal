using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoodTracker.Controls
{
    public partial class EmotionPieChart : UserControl
    {
        public SeriesCollection PieSeriesCollection { get; set; }

        public EmotionPieChart()
        {
            InitializeComponent();
            LoadChartData();
            DataContext = this;
        }

        private void LoadChartData()
        {
            PieSeriesCollection = new SeriesCollection
            {
                new PieSeries { Title = "快乐", Values = new ChartValues<double> { 30 }, Fill = Brushes.LightPink },
                new PieSeries { Title = "平静", Values = new ChartValues<double> { 25 }, Fill = Brushes.LightSkyBlue },
                new PieSeries { Title = "疲惫", Values = new ChartValues<double> { 20 }, Fill = Brushes.Khaki },
                new PieSeries { Title = "紧张", Values = new ChartValues<double> { 25 }, Fill = Brushes.MediumOrchid }
            };
        }
    }
}

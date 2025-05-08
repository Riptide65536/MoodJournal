using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MoodTracker.Controls
{
    public partial class EmotionLineChart : UserControl
    {
        public SeriesCollection LineSeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public EmotionLineChart()
        {
            InitializeComponent();
            LoadChartData();
            DataContext = this;
        }

        private void LoadChartData()
        {
            LineSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "平静",
                    Values = new ChartValues<double> { 100, 120, 90, 150, 130, 160 }
                },
                new LineSeries
                {
                    Title = "焦虑",
                    Values = new ChartValues<double> { 60, 70, 85, 50, 95, 65 }
                }
            };

            Labels = new List<string> { "周一", "周二", "周三", "周四", "周五", "周六" };
            YFormatter = value => value.ToString("N0");
        }
    }
}

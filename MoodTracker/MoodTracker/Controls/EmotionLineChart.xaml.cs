using LiveCharts;
using LiveCharts.Wpf;
using MoodTracker.Converters;
using MoodTracker.Data;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MoodTracker.Controls
{
    public partial class EmotionLineChart : UserControl
    {
        public string currentUserId = "0";
        public SeriesCollection? LineSeriesCollection { get; set; }
        public List<string> Labels { get; set; } = [];
        public Func<double, string>? YFormatter { get; set; }

        public EmotionLineChart()
        {
            InitializeComponent();
            LoadChartData();
            DataContext = this;
        }

        private void LoadChartData()
        {
            using var db = new ApplicationDbContext();
            var records = db.MoodRecords
                .Where(r => r.UserId == currentUserId)
                .Where(r => r.Datetime >= DateTime.Now.AddDays(-7))
                .OrderBy(r => r.Datetime);


            ChartValues<double> values = [];
            foreach (var r in records)
            {
                values.Add(EmotionConverter.ConvertToValue(r.CurrentEmotion));
                Labels.Add(r.Datetime.ToString("MM/dd"));
            }

            LineSeriesCollection = new SeriesCollection();
            LineSeriesCollection.Add(new LineSeries
            {
                Title = "心情值",
                Values = values
            });

            YFormatter = value => value.ToString("N0");
        }
    }
}

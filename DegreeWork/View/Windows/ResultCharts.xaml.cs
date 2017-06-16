using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using OxyPlot;

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для ResultCharts.xaml
    /// </summary>
    public partial class ResultCharts : Window
    {
        public ResultCharts(List<double> _ratings)
        {
            InitializeComponent();
            InitializeGraphic(_ratings);  
        }

        public void InitializeGraphic(List <double> ratings)
        {
            OxyPlot.Wpf.Plot plot = new OxyPlot.Wpf.Plot();
            OxyPlot.Wpf.LineSeries series = new OxyPlot.Wpf.LineSeries();

            for (int i =0; i<ratings.Count; i++)
            {
                series.Items.Add(new DataPoint(i,ratings.ElementAt(i)));
            }

            plot.Series.Add(new OxyPlot.Wpf.LineSeries());
            plot.Series[0].ItemsSource = series.Items;

            plot.Axes.Add(new OxyPlot.Wpf.LinearAxis());
            plot.Axes[0].Position = OxyPlot.Axes.AxisPosition.Bottom;
            plot.Axes[0].Title = "Номер популяции";

            plot.Axes.Add(new OxyPlot.Wpf.LinearAxis());
            plot.Axes[1].Position = OxyPlot.Axes.AxisPosition.Left;
            plot.Axes[1].Title = "Целевая функция";

            chartGrid.Children.Add(plot);
        }
       
    }

   
}

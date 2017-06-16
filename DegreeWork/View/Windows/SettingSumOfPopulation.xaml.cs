using DegreeWork.SpaceParam;
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

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для SettingSumOfPopulation.xaml
    /// </summary>
    public partial class SettingSumOfPopulation : Window
    {
        public SettingSumOfPopulation()
        {
            InitializeComponent();
        }

        private void sumOfPopulationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != ",") e.Handled = true;
        }

        private void setSumOfPopulationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(sumOfPopulationTextBox.Text))
            {
                SingleSpaceParams.getInstance().SumOfChromosomeInPopulation = int.Parse(sumOfPopulationTextBox.Text);
                this.Close();
            }
            
        }
    }
}

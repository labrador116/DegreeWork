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
    /// Логика взаимодействия для setProbabilityOfMutation.xaml
    /// </summary>
    public partial class SettingProbabilityOfMutation : Window
    {
        public SettingProbabilityOfMutation()
        {
            InitializeComponent();
        }

        private void probabilityOfMutationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != ",") e.Handled = true;
        }

        private void setProbabilityOfMutationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(probabilityOfMutationTextBox.Text))
            {
                SingleSpaceParams.getInstance().PropabilityOfMutation = double.Parse(probabilityOfMutationTextBox.Text);
                this.Close();
            }
        }
    }
}

using DataBaseStruct;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для CreateProjectPage.xaml
    /// </summary>
    public partial class CreateProjectPage : Page
    {
        DB_ConnectionContext _Context;
        public CreateProjectPage()
        {
            InitializeComponent();
           
            if (createCustomerTextBlock.IsEnabled == false)
            {
                createCustomerTextBlock.Opacity=0.1;
            }
        }

        private void ConnectToBdTextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ConnectToDBWindows dialogConnect = new ConnectToDBWindows();
            dialogConnect.GettingContext += DialogConnect_GettingContext;
            dialogConnect.ShowDialog();
        }

        private void DialogConnect_GettingContext(DB_ConnectionContext context)
        {
            _Context = context;

            if (_Context != null)
            {
                ConnectedToDbLabel.Content += "\n DataBase name: " + _Context.Database.Connection.Database;
                ConnectedToDbLabel.Visibility = Visibility.Visible;
                createCustomerTextBlock.IsEnabled = true;
                createCustomerTextBlock.Opacity = 1;
            }
        }
    }
}

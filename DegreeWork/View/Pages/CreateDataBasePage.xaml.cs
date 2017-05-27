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
using static DataBaseStruct.Model;

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для CreateDataBasePage.xaml
    /// </summary>
    public partial class CreateDataBasePage : Page
    {
        public CreateDataBasePage()
        {
            InitializeComponent();
        }

        private void CreateDataBaseButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            String connectionString = null;
            if (!String.IsNullOrEmpty(DataBaseNameTextBox.Text))
            {
                try
                {
                    DB_ConnectionContext DB_context = new DB_ConnectionContext(DataBaseNameTextBox.Text);
                    DB_context.Database.CreateIfNotExists();
                    connectionString = DB_context.Database.Connection.ConnectionString;
                }
                catch 
                {
                    ResultOfCreationDataBaseLabel.Content = "Ошибка! База данных не создана.";
                    ResultOfCreationDataBaseLabel.Visibility = Visibility.Visible;
                    return;
                }
                ResultOfCreationDataBaseLabel.Content = "База данных успешно создана!";
                ResultOfCreationDataBaseLabel.Visibility = Visibility.Visible;
                connectionStringTextBox.Text = connectionString;
                connectionStringTextBox.Visibility = Visibility.Visible;

            }
        }
    }
    
}

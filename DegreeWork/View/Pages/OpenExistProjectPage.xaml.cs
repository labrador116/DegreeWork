using DataBaseStruct;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static DataBaseStruct.Model;

namespace DegreeWork
{ 
    /// <summary>
    /// Логика взаимодействия для OpenExistProjectPage.xaml
    /// </summary>
    public partial class OpenExistProjectPage : Page
    {
        DB_ConnectionContext _Context;
        List<Project> _ProjectsFromDB;
        int _SelectedItemInCombobox;
        NavigationService nav;
        public OpenExistProjectPage()
        {
            InitializeComponent();
            SingleSpaceParams.KillSingle();

            if (chooseProjectTextBlock.IsEnabled == false)
            {
                chooseProjectTextBlock.Opacity = 0.1;
            }
        }

        private void ConnectToBdTextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ConnectToDBWindows dialogConnect = new ConnectToDBWindows();
            dialogConnect.GettingContext += DialogConnect_GettingContext;
            dialogConnect.ShowDialog();
        }

        private void DialogConnect_GettingContext(DataBaseStruct.DB_ConnectionContext context)
        {
            _Context = context;

            if (_Context != null)
            {
                ConnectedToDbLabel.Content += "\n DataBase name: " + _Context.Database.Connection.Database;
                ConnectedToDbLabel.Visibility = Visibility.Visible;
                chooseProjectTextBlock.Opacity = 1;
                chooseProjectComboBox.Visibility = Visibility.Visible;
            }
        }

        private void chooseProjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            _SelectedItemInCombobox = cb.SelectedIndex;
            OpenExistProjectButton.Visibility = Visibility.Visible;
        }

        private void OpenExistProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Project project = _ProjectsFromDB.ElementAt(_SelectedItemInCombobox);
                SchemeOfBuilding scheme = project.Scheme.First();
                if (SingleSpaceParams.getInstance() == null)
                {
                    SingleSpaceParams.getInstance(Convert.ToInt32(scheme.Width), Convert.ToInt32(scheme.Height));
                }
                else
                {
                    SingleSpaceParams.getInstance().Width = Convert.ToInt32(scheme.Width);
                    SingleSpaceParams.getInstance().Height = Convert.ToInt32(scheme.Height);
                }

                MainWindowPage mainWindow = new MainWindowPage(_Context, project.ProjectId);
                mainWindow.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка! Информация не была получена из базы данных!");
            }
        }
        
        private void chooseProjectComboBox_DropDownOpened(object sender, EventArgs e)
        {
            chooseProjectComboBox.Items.Clear();
            
            _ProjectsFromDB = _Context.Projects.ToList();

            foreach (Project project in _ProjectsFromDB)
            {
                chooseProjectComboBox.Items.Add(project.ProgectName);
            }
        }
    }
}

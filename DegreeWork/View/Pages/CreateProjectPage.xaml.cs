using DataBaseStruct;
using DegreeWork.SpaceParam;
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
using static DataBaseStruct.Model;

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для CreateProjectPage.xaml
    /// </summary>
    public partial class CreateProjectPage : Page
    {
        DB_ConnectionContext _Context;
        List<Customer> _СustomersFromDB;
        int _SelectedItemInCombobox;
        string _ProjectName;
        int _ProjectNumber;
        int _WidthOfArea;
        int _HeightOfArea;
        NavigationService nav;
        public CreateProjectPage()
        {
            InitializeComponent();
            SingleSpaceParams.KillSingle();

            if (createCustomerTextBlock.IsEnabled == false)
            {
                createCustomerTextBlock.Opacity=0.1;
            }

            if (chooseCustomerTextBlock.IsEnabled == false)
            {
                chooseCustomerTextBlock.Opacity = 0.1;
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
                chooseCustomerTextBlock.Opacity = 1;
                chooseCustomerComboBox.Visibility = Visibility.Visible;

                _СustomersFromDB = _Context.Customers.ToList();
                
                foreach(Customer customer in _СustomersFromDB )
                {
                    chooseCustomerComboBox.Items.Add(customer.Surname + " " + customer.Name + " " + customer.Patronymic);
                }
            }
        }

        private void createCustomerTextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CreateCustomerWindows dialogCreateCustomer = new CreateCustomerWindows(_Context);
            dialogCreateCustomer.ShowDialog();
        }

        private void chooseCustomerComboBox_DropDownOpened(object sender, EventArgs e)
        {
            chooseCustomerComboBox.Items.Clear();
            if (_Context.Customers.ToList() != null)
            {
                _СustomersFromDB = _Context.Customers.ToList();
            }

            foreach (Customer customer in _СustomersFromDB)
            {
                chooseCustomerComboBox.Items.Add(customer.Surname + " " + customer.Name + " " + customer.Patronymic);
            }
        }

            private void chooseCustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            _SelectedItemInCombobox = cb.SelectedIndex;

            NameOfProjectTextBlock.Visibility = Visibility.Visible;
            ProjectNameTextBox.Visibility = Visibility.Visible;
            DimensionsOfRoomTextBlock.Visibility = Visibility.Visible;
            WidthOfAreaTextBlock.Visibility = Visibility.Visible;
            WidthOfAreaTextBox.Visibility = Visibility.Visible;
            HeightOfAreaTextBlock.Visibility = Visibility.Visible;
            HeightOfAreaTextBox.Visibility = Visibility.Visible;
            SaveDataButton.Visibility = Visibility.Visible;
            ProjectNumbertTextBlock.Visibility = Visibility.Visible;
            ProjectNumberTextBox.Visibility = Visibility.Visible;
        }

        private void WidthOfAreaTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void HeightOfAreaTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
        private void ProjectNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ProjectNameTextBox.Text))
            {
                _ProjectName = ProjectNameTextBox.Text;
            }

            if (!String.IsNullOrEmpty(WidthOfAreaTextBox.Text))
            {
                _WidthOfArea = Convert.ToInt32(WidthOfAreaTextBox.Text);
            }

            if (!String.IsNullOrEmpty(HeightOfAreaTextBox.Text))
            {
                _HeightOfArea = Convert.ToInt32(HeightOfAreaTextBox.Text);
            }

            if (!String.IsNullOrEmpty(ProjectNumberTextBox.Text))
            {
                _ProjectNumber = Convert.ToInt32(ProjectNumberTextBox.Text);
            }

            if (!String.IsNullOrEmpty(_ProjectName) && _WidthOfArea!=0 && _HeightOfArea != 0 && _ProjectNumber != 0)
            {
                List<Customer> customers = _Context.Customers.ToList();
                Customer customer = customers.ElementAt(_SelectedItemInCombobox);

                Project project = new Project();
                project.Customer = customer;
                project.ProgectName = _ProjectName;
                project.ProjectNumber = _ProjectNumber;

                SchemeOfBuilding scheme = new SchemeOfBuilding();
                scheme.Width = _WidthOfArea;
                scheme.Height = _HeightOfArea;
                scheme.ProjectNumber = project;
                scheme.Point = new List<ControlPoint>();
                scheme.Rooms = new List<Room>();

                try
                {
                    _Context.Schemes.Add(scheme);
                    _Context.SaveChanges();

                    if (SingleSpaceParams.getInstance() == null)
                    {
                        SingleSpaceParams.getInstance(_WidthOfArea, _HeightOfArea);
                    }
                    else
                    {
                        SingleSpaceParams.getInstance().Width = _WidthOfArea;
                        SingleSpaceParams.getInstance().Height = _HeightOfArea;
                    }
                    nav = NavigationService.GetNavigationService(this);
                    
                    MainWindowPage mainWindow = new MainWindowPage(_Context, project.ProjectId);
                    mainWindow.Show();
                }
                catch
                {
                    MessageBox.Show("Ошибка! Информация не была сохранена в базу данных!");
                }
            }
        } 
    }
}

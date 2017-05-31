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
    /// Логика взаимодействия для FPage.xaml
    /// </summary>
    public partial class FirstPage : Page
    {
        private NavigationService nav;
        public FirstPage()
        {
            InitializeComponent();
        }

        private void createProject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new System.Uri("View/Pages/CreateProjectPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void createDataBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new System.Uri("View/Pages/CreateDataBasePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void openProject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new System.Uri("View/Pages/OpenExistProjectPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}

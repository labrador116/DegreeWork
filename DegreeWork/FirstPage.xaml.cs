﻿using System;
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
            CreateProjectPage page = new CreateProjectPage();
            nav.Navigate(page);
           // nav.Navigate(new System.Uri("CreateProjectPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}

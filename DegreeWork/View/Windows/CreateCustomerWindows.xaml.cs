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
using System.Windows.Shapes;
using static DataBaseStruct.Model;

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для CreateCustomerWindows.xaml
    /// </summary>
    public partial class CreateCustomerWindows : Window
    {
        DB_ConnectionContext _Context;
        public CreateCustomerWindows(DB_ConnectionContext context)
        {
            InitializeComponent();
            _Context = context;
        }

        private void saveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(customerNameTextBox.Text)
                && !String.IsNullOrEmpty(customerNameTextBox.Text)
                && !String.IsNullOrEmpty(customerPatronymicTextBox.Text))
            {
                Customer customer = new Customer
                {
                    Name = customerNameTextBox.Text,
                    Surname = customerSurnameTextBox.Text,
                    Patronymic = customerPatronymicTextBox.Text
                };

                try
                {
                    _Context.Customers.Add(customer);
                    _Context.SaveChanges();
                    this.Close();
                }
                catch
                {
                    customerWrongAddedLabel.Visibility = Visibility.Visible;
                }
            }
        }
    }
}

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

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для ConnectToDB.xaml
    /// </summary>
    public partial class ConnectToDBWindows : Window
    {
        public delegate void GetContextDelegate(DB_ConnectionContext context);
        public event GetContextDelegate GettingContext;

        DB_ConnectionContext _Context;

        public ConnectToDBWindows()
        {
            InitializeComponent();
        }

        public DB_ConnectionContext Context { get => _Context; set => _Context = value; }

        private void ContentStringToDBTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ContentStringToDBTextBox.Text = String.Empty;
        }
   
        private void ConnectToDBButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ContentStringToDBTextBox.Text))
                {
                    _Context = new DB_ConnectionContext(ContentStringToDBTextBox.Text);
                    if (!_Context.Database.Exists())
                    {
                        throw new Exception();
                    }
                }
                GettingContext(_Context);
                this.Close();
            }
            catch
            {
                ResultConnectToDBLabel.Visibility = Visibility.Visible;
            }
        }
    }
}

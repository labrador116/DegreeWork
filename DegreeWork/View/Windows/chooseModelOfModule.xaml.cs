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
    /// Логика взаимодействия для chooseModelOfModule.xaml
    /// </summary>
    public partial class ChooseModelOfModule : Window
    {
        public delegate void SendSelectedModules(ModelsOfModules module);
        public event SendSelectedModules SendSelected;

        List<ModelsOfModules> _Models;
        List<ModelsOfModules> _SelectrdModules = new List<ModelsOfModules>();
        public ChooseModelOfModule(List<ModelsOfModules> models)
        {
            InitializeComponent();
            _Models = models;
            InitListBox();
        }

        private void InitListBox()
        {
            if (_Models != null)
            {
                if (_Models.Count > 0)
                {
                    foreach(ModelsOfModules model in _Models)
                    {
                        chooseModelOfModulesListBox.Items.Add(model.ModelName + " Радиус: " + model.ModelRadius);
                    }
                }
            }
        }
        
        private void chooseModelOfModulesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            SendSelected(_Models.ElementAt(lb.SelectedIndex));
            this.Close();
        }
    }
}

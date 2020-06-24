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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int allCat;
        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = new ComboBoxViewModel();
        }

        private void r1_Checked(object sender, RoutedEventArgs e)
        {
            allCat = 1;
            l1.Content = allCat;
        }

        private void r2_Checked(object sender, RoutedEventArgs e)
        {
            allCat = 2;
            l1.Content = allCat;
        }

        private void comboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            l2.Content = comboBoxCategory.SelectedItem.ToString();
            
        }

        private void cb_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void cb_1_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            
        }

        private void cb_1_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void cb_1_DropDownClosed(object sender, EventArgs e)
        {
            tb1.Text = cb_1.Text;
        }
    }
}

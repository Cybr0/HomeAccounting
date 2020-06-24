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

namespace HomeAccounting
{
    /// <summary>
    /// Логика взаимодействия для ha_add_new_entry.xaml
    /// </summary>
    public partial class ha_add_new_entry : Window
    {
        public ha_add_new_entry()
        {
            InitializeComponent();
        }

        public void SetContent(string content)
        {
            this.Content = content;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (Application.Current as App).New_entries.Remove(this);
        }
    }
}

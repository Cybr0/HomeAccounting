using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HomeAccounting
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private List<ha_add_new_entry> new_entries = new List<ha_add_new_entry>();
        public List<ha_add_new_entry> New_entries
        {
            get { return new_entries; }
            set { new_entries = value; }
        }


        private List<ha_statistics> statistics = new List<ha_statistics>();
        public List<ha_statistics> Statistics
        {
            get { return statistics; }
            set { statistics = value; }
        }
    }
}

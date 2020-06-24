using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class ComboBoxViewModel
    {
        public List<string> CategoryNameCollection { get; set; }
        public ComboBoxViewModel()
        {
            CategoryNameCollection = new List<string>()
            {
                "Зарплата",
                "Продуткы",
                "Интернет"
            };
        }
    }
}

using emplySoftware.Class;
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

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditTaskWindow.xaml
    /// </summary>
    public partial class AddEditTaskWindow : Window
    {
        private List<string> listUsers = new List<string>();
        public AddEditTaskWindow()
        {
            InitializeComponent();
            foreach (var user in App.ContextDatabase.User.ToList())
            {
                string mfl = FIOus.GetFullName(user);
                listUsers.Add(mfl);
            }
            combo_box_task_user.ItemsSource = listUsers;
        }
    }
}

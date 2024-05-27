using emplySoftware.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private List<string> listUsers = new List<string>();
        public Dashboard()
        {
            InitializeComponent();
            foreach (var user in App.ContextDatabase.User.ToList())
            {
                string mfl = FIOus.GetFullName(user);
                listUsers.Add(mfl);
            }
            user_chart_combo_box.ItemsSource = listUsers;
        }
    }
}

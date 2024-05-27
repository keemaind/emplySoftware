using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
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
using System.Windows.Forms.DataVisualization.Charting;
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
            type_chart_combo_box.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void user_chart_combo_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<User> usersBD = App.ContextDatabase.User.ToList();
            if (user_chart_combo_box.SelectedItem.ToString() is string user &&
                type_chart_combo_box.SelectedItem is SeriesChartType currentType)
            {
                User userG = usersBD.First(p => p.GetFullName() == user);
                Series currentSeries = ChartTask.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                int userID = userG.userID;

                var tasks = App.ContextDatabase.Task.ToList();
                foreach (var task in tasks)
                {
                    if (task.EmployeeID == userID)
                    {
                        currentSeries.Points.AddXY(task.Title, task.Difficulty);
                    }
                }
            }
        }
    }
}

using emplySoftware.Class;
using System;
using System.CodeDom.Compiler;
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
using emplySoftware.DatabaseSQL;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditTaskWindow.xaml
    /// </summary>
    public partial class AddEditTaskWindow : Window
    {
        private List<string> listUsers = new List<string>();
        private DatabaseSQL.Task _currentTask = null;
        private bool isEditing;
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

        public AddEditTaskWindow(DatabaseSQL.Task task)
        {
            InitializeComponent();

            _currentTask = task;

            if (_currentTask.Title != null)
            {
                isEditing = true;
                text_box_task_name.Text = _currentTask.Title.ToString();
                text_box_task_description.Text = _currentTask.Description.ToString();
                date_picker_create_date.Text = _currentTask.CreateDate.ToString();
                date_picker_task_deadline.Text = _currentTask.Deadline.ToString();
                combo_box_task_status.Text = _currentTask.Status.ToString();
                text_box_task_difficulty.Text = _currentTask.Difficulty.ToString();
                foreach (var user in App.ContextDatabase.User.ToList())
                {
                    string mfl = FIOus.GetFullName(user);
                    listUsers.Add(mfl);
                }
                var userTask = App.ContextDatabase.User.FirstOrDefault(p => p.userID == _currentTask.EmployeeID);
                combo_box_task_user.ItemsSource = listUsers;
                combo_box_task_user.Text = FIOus.GetFullName(userTask).ToString();

            }

        }

        private void add_edit_task_close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {




        }
    }
}

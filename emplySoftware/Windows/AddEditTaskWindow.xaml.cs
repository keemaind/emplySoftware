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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Entity.Migrations;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditTaskWindow.xaml
    /// </summary>
    public partial class AddEditTaskWindow : System.Windows.Window
    {
        public event EventHandler DataChanged;
        private List<string> listUsers = new List<string>();
        private TaskList _currentTask = null;
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

        public AddEditTaskWindow(TaskList task)
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
        private int GGuserID;
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                
                if (isEditing == false)
                {

                    string FIOusCombobox = combo_box_task_user.SelectedItem.ToString();
                    var usList = App.ContextDatabase.User.ToList();
                    foreach (var us in usList)
                    {
                        if (FIOus.GetFullName(us) == FIOusCombobox)
                        {
                            GGuserID = us.userID;
                            break;
                        }
                    }
                    var task = new DatabaseSQL.Task
                    {
                        Title = text_box_task_name.Text.ToString(),
                        Description = text_box_task_description.Text.ToString(),
                        CreateDate = Convert.ToDateTime(date_picker_create_date.Text),
                        Deadline = Convert.ToDateTime(date_picker_task_deadline.Text),
                        Difficulty = Convert.ToDouble(text_box_task_difficulty.Text),
                        Status = combo_box_task_status.Text,
                        EmployeeID = GGuserID
                    };
                    App.ContextDatabase.Task.Add(task);
                    App.ContextDatabase.SaveChanges();
                    DataChanged?.Invoke(this, new EventArgs());
                    this.Close();
                }
                else
                {
                    var existingTask = App.ContextDatabase.Task.FirstOrDefault(t => t.ID == _currentTask.ID);
                    string FIOusCombobox = combo_box_task_user.SelectedItem.ToString();
                    var usList = App.ContextDatabase.User.ToList();
                    foreach ( var us in usList )
                    {
                        if (FIOus.GetFullName(us) == FIOusCombobox)
                        {
                            GGuserID = us.userID;
                            break;
                        }
                    }

                    if (existingTask != null)
                    {
                        existingTask.Title = text_box_task_name.Text.ToString();
                        existingTask.Description = text_box_task_description.Text.ToString();
                        existingTask.CreateDate = Convert.ToDateTime(date_picker_create_date.Text);
                        existingTask.Deadline = Convert.ToDateTime(date_picker_task_deadline.Text);
                        existingTask.Difficulty = Convert.ToDouble(text_box_task_difficulty.Text);
                        existingTask.Status = combo_box_task_status.Text;
                        existingTask.EmployeeID = GGuserID;
                        App.ContextDatabase.Task.AddOrUpdate(existingTask);
                        App.ContextDatabase.SaveChanges();
                        DataChanged?.Invoke(this, new EventArgs());
                        this.Close();

                    }
                }

            }

           
        }
        public string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(text_box_task_name.Text)) errorBuilder.AppendLine("Название услуги обязательно для заполнения;");
            var serviceFromDB = App.ContextDatabase.Task.ToList().FirstOrDefault(p => p.Title.ToLower() == text_box_task_name.Text.ToLower());
            int complexity = 0;
            if (int.TryParse(text_box_task_difficulty.Text, out complexity) == false || complexity <= 0)
                errorBuilder.AppendLine("Сложность должна быть положительным числом;");
            if (int.TryParse(text_box_task_difficulty.Text, out complexity) == false || complexity > 50)
                errorBuilder.AppendLine("Сложность заключена в диапазоне от 1 до 50;");
            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки: \n");
            return errorBuilder.ToString();
        }
    }
}

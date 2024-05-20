using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
using emplySoftware.Class;
using emplySoftware.Windows;
using System.ComponentModel;

namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : Page
    {
        public Tasks()
        {
            InitializeComponent();
            //LoadGrid(GetCurrent.CurrentUser);
            HELP();
        }
        private void HELP()
        {
            pbCalculationProgress.Value = 0;
            data_grid_task.Items.Clear();

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(20);
        }
        List<TaskList> data_grid_tasks = new List<TaskList>();
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            
            var taskList = App.ContextDatabase.Task.Where(x => x.EmployeeID == GetCurrent.CurrentUser.userID).ToList();
            int max = taskList.Count;
            string FioUs = FIOus.GetFullName(GetCurrent.CurrentUser);
            foreach (var task in taskList)
            {
                i++;
                int progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                data_grid_tasks.Add(new TaskList
                {
                    ID = task.ID,
                    EmployeeID = task.EmployeeID,
                    Title = task.Title,
                    Description = task.Description,
                    CreateDate = task.CreateDate,
                    Deadline = task.Deadline,
                    Difficulty = task.Difficulty,
                    Status = task.Status,
                    FioUser = FioUs
                });
                (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
            }
            e.Result = data_grid_tasks;
            
        }
        public int item = 0;
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            pbCalculationProgress.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                data_grid_task.Items.Add(data_grid_tasks[item]);
                item++;
            }
                
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }
        

        private void add_task_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            AddEditTaskWindow addEditTaskWindow = new AddEditTaskWindow();

            ApplyEffect(mainWindow);
            addEditTaskWindow.ShowDialog();
            ClearEffect(mainWindow);
        }
        private void ApplyEffect(Window win)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 4;
            win.Effect = objBlur;
        }
        private void ClearEffect(Window win)
        {
            win.Effect = null;
        }

        private void data_grid_edit_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            var currentTask = (sender as Button).DataContext as DatabaseSQL.Task;

            AddEditTaskWindow addEditTaskWindow = new AddEditTaskWindow(currentTask);
            ApplyEffect(mainWindow);
            addEditTaskWindow.ShowDialog();
            ClearEffect(mainWindow);

        }

        private void data_grid_delete_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            YesOrNoWindow yesOrNoWindow = new YesOrNoWindow();
            ApplyEffect(mainWindow);
            yesOrNoWindow.ShowDialog();
            ClearEffect(mainWindow);
        }

        private void combo_box_task_status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void search_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}

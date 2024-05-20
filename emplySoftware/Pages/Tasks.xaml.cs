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
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : Page
    {
        #region Визуал часть
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
        #endregion

        List<TaskList> data_grid_tasks = new List<TaskList>();
        List<TaskList> filtered_data_grid_tasks = new List<TaskList>();
        CollectionViewSource viewSource = new CollectionViewSource();

        public Tasks()
        {
            InitializeComponent();
            //LoadGrid(GetCurrent.CurrentUser);
            HELP();

        }
        #region Заполнение тасков
        private void HELP()
        {
            pbCalculationProgress.Value = 0;
            data_grid_tasks.Clear();
            data_grid_task.Items.Clear();
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(20);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            int progressPercentage = 0;
            var taskList = App.ContextDatabase.Task.Where(x => x.EmployeeID == GetCurrent.CurrentUser.userID).ToList();
            int max = taskList.Count;
            string FioUs = FIOus.GetFullName(GetCurrent.CurrentUser);
            
            foreach (var task in taskList)
            {
                i++;
                progressPercentage = Convert.ToInt32(((double)i / max) * 100);
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
            item = 0;
        }
        #endregion 

        private void add_task_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            AddEditTaskWindow addEditTaskWindow = new AddEditTaskWindow();

            ApplyEffect(mainWindow);
            addEditTaskWindow.DataChanged += P1_DataChanged;
            addEditTaskWindow.ShowDialog();
            ClearEffect(mainWindow);
        }


        private void data_grid_edit_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            var currentTask = (sender as Button).DataContext as TaskList;

            AddEditTaskWindow addEditTaskWindow = new AddEditTaskWindow(currentTask);
            ApplyEffect(mainWindow);
            addEditTaskWindow.DataChanged += P1_DataChanged;
            addEditTaskWindow.ShowDialog();
           
            ClearEffect(mainWindow);
            
        }
        private void P1_DataChanged(object sender, EventArgs e)
        {
            HELP();
        }

        private void data_grid_delete_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            YesOrNoWindow yesOrNoWindow = new YesOrNoWindow();
            ApplyEffect(mainWindow);
            yesOrNoWindow.ShowDialog();
            bool choice = yesOrNoWindow.choice;
            if (choice)
            {

            }
            else

                ClearEffect(mainWindow);
        }


        private void combo_box_task_status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ComboBoxItem selectedValue = (ComboBoxItem)combo_box_task_status.SelectedItem;

            filtered_data_grid_tasks.Clear();
            data_grid_task.Items.Clear();
            if (selectedValue.Content.ToString() == "Все")
            {
                HELP();
            }
            else
            {

                var taskList = App.ContextDatabase.Task.Where(x => x.EmployeeID == GetCurrent.CurrentUser.userID && x.Status == selectedValue.Content.ToString()).ToList();
                string FioUs = FIOus.GetFullName(GetCurrent.CurrentUser);

                foreach (var task in taskList)
                {
                    filtered_data_grid_tasks.Add(new TaskList
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
                }
                for(int i=0; i< filtered_data_grid_tasks.Count; i++ )
                {
                    data_grid_task.Items.Add(filtered_data_grid_tasks[i]);
                }

            }
        }

        private bool UserFilterSearch(object item)
        {
            if (String.IsNullOrEmpty(search_text_box.Text))
                return true;
            else
                return ((item as TaskList).Title.IndexOf(search_text_box.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

       

        private void search_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(data_grid_task.Items);
            view.Filter = UserFilterSearch;
        }

    }
}


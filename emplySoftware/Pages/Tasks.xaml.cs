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

            LoadGrid(GetCurrent.CurrentUser);
        }
        
        
        private void LoadGrid(User user)
        {
            data_grid_task.ItemsSource = tasks.TaskFills(GetCurrent.CurrentUser);
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

        }
    }
}

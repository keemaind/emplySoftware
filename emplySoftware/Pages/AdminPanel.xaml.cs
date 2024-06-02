using emplySoftware.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdmiPanel.xaml
    /// </summary>
    public partial class AdmiPanel : Page
    {
        List<UserList> data_grid_users = new List<UserList>();
        List<UserList> filtered_data_grid_users = new List<UserList>();
        public AdmiPanel()
        {
            InitializeComponent();
            HELP();
        }
        #region Заполнение пользователей

        private void HELP()
        {
            loadSpinner.Visibility = Visibility.Visible;
            data_grid_users.Clear();
            data_grid_user.Items.Clear();
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
            var userList = App.ContextDatabase.User.ToList();
            int max = userList.Count;
            
            foreach (var user in userList)
            {
                i++;
                progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                data_grid_users.Add(new UserList
                {
                    userID = user.userID,
                    Login = user.Login,
                    StartSession = (DateTime)user.StartSession,
                    Status = (bool)user.Banned
                });
                (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
            }
            e.Result = data_grid_users;

        }
        public int item = 0;
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.UserState != null)
            {
                data_grid_user.Items.Add(data_grid_users[item]);
                item++;
            }

        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadSpinner.Visibility = Visibility.Collapsed;
            item = 0;
        }
        #endregion

        private void ban_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            var currentUser = (sender as Button).DataContext as UserList;
            var user = App.ContextDatabase.User.FirstOrDefault(x => x.userID == currentUser.userID);
            BanOrNoWindow yesOrNoWindow = new BanOrNoWindow();
            ApplyEffect(mainWindow);
            yesOrNoWindow.ShowDialog();
            bool choice = yesOrNoWindow.choice;
            if (choice)
            {
                user.Banned = (bool)choice;
                App.ContextDatabase.SaveChanges();
                ClearEffect(mainWindow);
            }
            else
                ClearEffect(mainWindow);
        }

        private void unBan_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            var currentUser = (sender as Button).DataContext as UserList;
            var user = App.ContextDatabase.User.FirstOrDefault(x => x.userID == currentUser.userID);
            string g = "Пользователь разбанен";
            NotificationWindow notificationWindow = new NotificationWindow(g);
            notificationWindow.ShowDialog();
            user.Banned = false;
            App.ContextDatabase.SaveChanges();
            
            
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

        private bool UserFilterSearch(object item)
        {
            if (String.IsNullOrEmpty(search_text_box.Text))
                return true;
            else
                return ((item as UserList).Login.IndexOf(search_text_box.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void search_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(data_grid_user.Items);
            view.Filter = UserFilterSearch;
        }

        private void combo_box_user_status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedValue = (ComboBoxItem)combo_box_user_status.SelectedItem;

            filtered_data_grid_users.Clear();
            data_grid_user.Items.Clear();
            if (selectedValue.Content.ToString() == "Все")
            {
                HELP();
            }
            else
            {

                var userList = App.ContextDatabase.User.Where(x => x.Banned == true);

                foreach (var user in userList)
                {

                    filtered_data_grid_users.Add(new UserList
                    {
                        userID = user.userID,
                        Login = user.Login,
                        StartSession = (DateTime)user.StartSession,
                        Status = (bool)user.Banned
                    });
                    
                }

                for (int i = 0; i < filtered_data_grid_users.Count; i++)
                {
                    data_grid_user.Items.Add(filtered_data_grid_users[i]);
                }

            }
        }

        private void statisticsButton_Click(object sender, RoutedEventArgs e)
        {
            ChatsDashboardWindow dashboardWindow = new ChatsDashboardWindow();
            dashboardWindow.ShowDialog();
        }
    }
}

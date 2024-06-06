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
using Windows.System;

namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        #region BlurEffect
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

        List<UserList> data_grid_users = new List<UserList>();
        List<UserList> filtered_data_grid_users = new List<UserList>();
        CollectionViewSource viewSource = new CollectionViewSource();

        public UsersPage()
        {
            InitializeComponent();
            Filling();
        }

        #region Filling
        private void Filling()
        {
            pbCalculationProgress.Value = 0;
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
            var positionList = App.ContextDatabase.Position.ToList();
            int max = userList.Count;

            foreach (var user in userList)
            {
                i++;
                progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                data_grid_users.Add(new UserList
                {
                    userID = user.userID,
                    imageUser = user.Image,
                    Position = positionList.First(p => p.ID == user.PositionID).Title,
                    FioUser = user.MiddleName + " " + user.FirstName + " " + user.LastName,
                });
                (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
            }
            e.Result = data_grid_users;

        }
        public int item = 0;
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            pbCalculationProgress.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                data_grid_user.Items.Add(data_grid_users[item]);
                item++;
            }

        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            item = 0;
        }
        #endregion

        private void Search_text_box_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Add_user_button_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Data_grid_edit_button_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Data_grid_delete_button_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

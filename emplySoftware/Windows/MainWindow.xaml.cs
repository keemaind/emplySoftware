using emplySoftware.Class;
using emplySoftware.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using emplySoftware.DatabaseSQL;
using System.Diagnostics;
using System.Collections;
using emplySoftware.Pages;
using System.ComponentModel;
using System.Collections.ObjectModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;
using System.Runtime.InteropServices;
using System.Xml.Linq;


namespace emplySoftware
{

    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            MainWindowModel model = new MainWindowModel();
            DataContext = model;
            var userImage = App.ContextDatabase.User.Where(p => p.userID == GetCurrent.CurrentUser.userID).ToList();
            foreach (var user in userImage)
            {
                UserImage.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(user.Image);
                ProfileFirstName.Text = user.FirstName;
                ProfileMiddleName.Text = user.MiddleName;
            }
            main_frame.Navigate(new MainPage());
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MainCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseMenuBar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEditProfile_Click(object sender, RoutedEventArgs e)
        {
            UserSettingsWindow userSettingsWindow = new UserSettingsWindow(GetCurrent.CurrentUser);
            userSettingsWindow.Owner = this;
            ApplyEffect(this);
            userSettingsWindow.ShowDialog();
            ClearEffect(this);
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        //Изменение чата по выбору в listView
        private void UserChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedChat = (UserChats.SelectedItem as chats);
            main_frame.NavigationService.Navigate(new ChatPage(selectedChat));

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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

        ObservableCollection<string> items = new ObservableCollection<string>();

        // Создание CollectionViewSource



        private void ButtonTasks_Click(object sender, RoutedEventArgs e)
        {
            main_frame.NavigationService.Navigate(new Tasks());
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            ChatCreatePage p1 = new ChatCreatePage();
            p1.DataChanged += P1_DataChanged;
            main_frame.Navigate(p1);
            //main_frame.NavigationService.Navigate(new ChatCreatePage());
        }
        private void P1_DataChanged(object sender, EventArgs e)
        {
            MainWindowModel model = new MainWindowModel();
            DataContext = model;
        }
    }

}

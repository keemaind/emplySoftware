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
        
        MainPage mainPage = new MainPage();
        public MainWindow()
        {
            InitializeComponent();
            MainWindowModel model = new MainWindowModel();
            DataContext = model;
            main_frame.Navigate(mainPage);
            
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
            userSettingsWindow.DataChanged += P1_DataChanged;
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
            main_frame.Navigate(mainPage);
        }


        private void search_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(UserChats.ItemsSource);
            view.Filter = UserFilter;
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(search_text_box.Text))
                return true;
            else
                return ((item as chats).Title.IndexOf(search_text_box.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void NewMessage_Change(object sender, EventArgs e)
        {
            
        }
        
        
    }

}

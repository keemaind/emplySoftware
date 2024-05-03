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
using System.Windows.Navigation;
using emplySoftware.DatabaseSQL;
using System.Diagnostics;
using System.Collections;
using emplySoftware.Pages;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace emplySoftware
{

    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            FillChats(GetCurrent.CurrentUser);
            var userImage = App.ContextDatabase.User.Where(p => p.userID == GetCurrent.CurrentUser.userID).ToList();
            foreach (var user in userImage)
            {
                UserImage.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(user.Image);
                ProfileFirstName.Text = user.FirstName;
                ProfileMiddleName.Text = user.MiddleName;
            }

            main_frame.Navigate(new Dashboard());
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            BorderMenuBar.Visibility = Visibility.Visible;
            ShadowOverlay.Visibility = Visibility.Visible;
            MainMenuButton.Visibility = Visibility.Collapsed;
            MainSearchButton.Visibility = Visibility.Collapsed;
        }
        List<chats> userChats = new List<chats>();
        

        private void FillChats(User currentUser)
        {
            //Заполнение созданных чатов если он личный, заполняется ФИО с кем ведется чат 
            var chatListPersonalCreate = App.ContextDatabase.chatList.Where(p => p.userID == currentUser.userID && p.personal == true);
            foreach (var chatPers in chatListPersonalCreate)
            {
                var personalUser = App.ContextDatabase.chatUsers.Where(p => p.chatID == chatPers.chatID);
                foreach (var chatUS in personalUser)
                {
                    var us = App.ContextDatabase.User.FirstOrDefault(p => p.userID == chatUS.userID);
                    string G = FIOus.GetNotFullName(us).ToString();

                    userChats.Add(new chats
                    {
                        Title = G,
                        Image = us.Image,
                        ChatID = chatPers.chatID,
                    });
                }
            }
            //Заполнение чатов где пользователь в чате с кем то
            var chatListPersonalGet = App.ContextDatabase.chatUsers.Where(p => p.userID == currentUser.userID);
            foreach (var chatPersGet in chatListPersonalGet)
            {
                var personalGet = App.ContextDatabase.chatList.Where(p => p.chatID == chatPersGet.chatID && p.personal == true);
                foreach (var chatUS in personalGet)
                {
                    var us = App.ContextDatabase.User.FirstOrDefault(p => p.userID == chatUS.userID);
                    string G = FIOus.GetNotFullName(us).ToString();
                    userChats.Add(new chats
                    {
                        Title = G,
                        Image = us.Image,
                        ChatID = chatUS.chatID
                    });
                }
            }
            //Заполнение созданных групповых чатов, заполняется название чата
            var chatListGroupCreate = App.ContextDatabase.chatList.Where(p => p.userID == currentUser.userID && p.personal == false);
            foreach (var chatGrop in chatListGroupCreate)
            {
                    userChats.Add(new chats
                    {
                        Title = chatGrop.Title,
                        Image = chatGrop.Image,
                        ChatID = chatGrop.chatID,
                    });
            }
            //Заполнение групповых чатов где находится пользователь, заполняется название чата
            var chatListGroupGet = App.ContextDatabase.chatUsers.Where(p => p.userID == currentUser.userID);
            foreach (var chatGrop in chatListGroupGet)
            {
                var chatGropGet = App.ContextDatabase.chatList.Where(p => p.chatID == chatGrop.chatID && p.personal == false);
                foreach (var chatGet in chatGropGet)
                {
                    userChats.Add(new chats
                    {
                        Title = chatGet.Title,
                        Image = chatGet.Image,
                        ChatID = chatGet.chatID
                    });
                }
            }
            UserChats.ItemsSource = userChats;
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
            BorderMenuBar.Visibility = Visibility.Collapsed;
            ShadowOverlay.Visibility = Visibility.Collapsed;
            MainMenuButton.Visibility = Visibility.Visible;
            MainSearchButton.Visibility = Visibility.Visible;

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

        private void MainSearchButton_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource viewSource = new CollectionViewSource();
            viewSource.Source = items;

            // Привязка ListView к CollectionViewSource
            UserChats.ItemsSource = viewSource.View;
            viewSource.View.Filter = obj => 
                {
                    string item = obj as string;
                    if (item == null) return false;
                    return item.Contains(MainSearchButton.Text);
                };
            }

        private void ButtonTasks_Click(object sender, RoutedEventArgs e)
        {
            main_frame.NavigationService.Navigate(new Tasks());
        }
    }

    }

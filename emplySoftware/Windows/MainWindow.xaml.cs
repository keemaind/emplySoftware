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

namespace emplySoftware
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
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
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
                BorderMenuBar.Visibility = Visibility.Visible;
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
                    string G = FIOus.GetFullName(us).ToString();

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
                    string G = FIOus.GetFullName(App.ContextDatabase.User.FirstOrDefault(p => p.userID == chatUS.userID));

                    userChats.Add(new chats
                    {
                        Title = G,
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
                MainMenuButton.Visibility = Visibility.Visible;
                MainSearchButton.Visibility = Visibility.Visible;
        }

        private void ButtonEditProfile_Click(object sender, RoutedEventArgs e)
        {
            UserSettingsWindow userSettingsWindow = new UserSettingsWindow(GetCurrent.CurrentUser);
            userSettingsWindow.Show();
        }
    }
    class chats
    {
        string title;
        int chatID;
        byte[] image;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public int ChatID
        {
            get { return chatID; }
            set { chatID = value; }
        }
        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }

    }
}

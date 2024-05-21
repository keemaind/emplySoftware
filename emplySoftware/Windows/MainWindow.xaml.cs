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
using System.Windows.Threading;
using System.Windows.Forms;


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
        #region Визуал часть
        private void MainMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MainCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        #endregion

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

        #region ЧАТ 
        //Изменение чата по выбору в listView
        private void UserChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedChat = (UserChats.SelectedItem as chats);
            var persOrGrop =  App.ContextDatabase.chatList.FirstOrDefault(p => p.chatID == selectedChat.ChatID).personal;
            if (persOrGrop.Value == true) ChatPersonalPage(selectedChat);
            else ChatGroupPage(selectedChat);


            //main_frame.NavigationService.Navigate(new ChatPage(selectedChat));

        }
        List<MessageList> MessageList = new List<MessageList>();

        public event EventHandler DataChanged;
        public int thisChatID;
        private DispatcherTimer timer;
        int msgCount;

        public void ChatPersonalPage(chats selectedChat)
        {
            thisChatID = selectedChat.ChatID;
            var chatUs = App.ContextDatabase.chatUsers.FirstOrDefault(p => p.chatID == thisChatID);
            var UsImg = App.ContextDatabase.User.FirstOrDefault(p => p.userID == chatUs.userID);

            var CurUsImg = App.ContextDatabase.User.FirstOrDefault(p => p.userID == GetCurrent.CurrentUser.userID);
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            MessagesListView.ItemsSource = MessageList;
            RefreshPersonalChat(thisChatID, UsImg.Image, CurUsImg.Image);
            msgCount = MessageList.Count();
        }
        private void RefreshPersonalChat(int thisChatID, byte[] UsImg, byte[] CurUsImg)
        {
            var MessagesBD = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
            int curUsInt = GetCurrent.CurrentUser.userID;
            foreach (var message in MessagesBD)
            {
                if (message.userID == curUsInt)
                {
                    MessageList.Add(new MessageList
                    {
                        messageID = message.messageID,
                        userID = (int)message.userID,
                        Message = message.Message,
                        sendDate = (DateTime)message.sendDate,
                        imageUser = CurUsImg
                    });
                }
                else
                    MessageList.Add(new MessageList
                    {
                        messageID = message.messageID,
                        userID = (int)message.userID,
                        Message = message.Message,
                        sendDate = (DateTime)message.sendDate,
                        imageUser = UsImg
                    });

            }
        }
        public void ChatGroupPage(chats selectedChat)
        {
            thisChatID = selectedChat.ChatID;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            MessagesListView.ItemsSource = MessageList;
            Refresh(thisChatID);
            msgCount = MessageList.Count();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            int msgCountRD = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).Count();
            if (msgCount != msgCountRD)
            {
                MessageList.Clear();
                Refresh(thisChatID);
                msgCount = MessageList.Count();
                DataChanged?.Invoke(this, new EventArgs());
            }
        }
        private void Refresh(int thisChatID)
        {
            var MessagesBD = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
            foreach (var message in MessagesBD)
            {
                MessageList.Add(new MessageList
                {
                    messageID = message.messageID,
                    userID = (int)message.userID,
                    Message = message.Message,
                    sendDate = (DateTime)message.sendDate,
                    
                });
            }
        }
        private void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            string msg = MsgTextBlock.Text.ToString();
            var us = App.ContextDatabase.User.FirstOrDefault(t => t.userID == GetCurrent.CurrentUser.userID);

            if (msg != "")
            {
                var Messages = new Messages
                {
                    chatID = thisChatID,
                    userID = us.userID,
                    Message = msg,
                    sendDate = DateTime.Now,
                };
                App.ContextDatabase.Messages.Add(Messages);
                App.ContextDatabase.SaveChanges();
                MsgTextBlock.Text = "";
                RefreshData();
                DataChanged?.Invoke(this, new EventArgs());
            }
            else
            {
                string errorMessage = "Сообщение не может быть пустым!";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.ShowDialog();
            }
        }
        #endregion ЧАТ 

        private void menu_button_Click(object sender, RoutedEventArgs e)
        {
            if (menu_button.IsChecked == true)
            {
                navigation_menu_text_block.Visibility = Visibility.Visible;
            }
            else
            {
                navigation_menu_text_block.Visibility = Visibility.Collapsed;
            }
        }
    }

}

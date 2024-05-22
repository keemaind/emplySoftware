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
        List<MessageList> MessagesList = new List<MessageList>();
        public event EventHandler DataChanged;
        private BackgroundWorker worker = null;
        public int thisChatID;
        private DispatcherTimer timer;
        int msgCount;
        public int curUserID;
        MainPage mainPage = new MainPage();
        public byte[] usImg = null;
        public byte[] curUsImg = null;

        public MainWindow()
        {
            InitializeComponent();
            MainWindowModel model = new MainWindowModel();
            DataContext = model;
            curUsImg = model.imageUserST;
            curUserID = GetCurrent.CurrentUser.userID;

            user_name_menu.Text = FIOus.GetNotFullName(GetCurrent.CurrentUser);
            
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

        #region Разные методы 
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
        #endregion 

        #region ЧАТ 

        List<GroupUserImages> usersImageList = new List<GroupUserImages>();
        DispatcherTimer Ltimer = new DispatcherTimer();
        //Изменение чата по выбору в listView
        private void UserChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Ltimer.IsEnabled) Ltimer.Stop();
            
            MessagesList.Clear();
            MessagesListView.Items.Clear();
            var selectedChat = (UserChats.SelectedItem as chats);
            var persOrGrop = App.ContextDatabase.chatList.FirstOrDefault(p => p.chatID == selectedChat.ChatID).personal;
            if (persOrGrop.Value == true) 
            {
                thisChatID = selectedChat.ChatID;
                usImg = selectedChat.Image;

                PersonalMessageAdd();
                main_frame.Visibility = Visibility.Hidden;
                sendBlock.Visibility = Visibility.Visible;
                MsgTextBlock.Visibility = Visibility.Visible;
                sendMessage.Visibility = Visibility.Visible;
            }
            else
            {
                thisChatID = selectedChat.ChatID;
                var usersImages = App.ContextDatabase.chatUsers.Where(p => p.chatID == selectedChat.ChatID).ToList();
                foreach (var usIm in usersImages)
                {
                    usersImageList.Add( new GroupUserImages
                    {
                        userID = (int)usIm.userID,
                        imageUser = App.ContextDatabase.User.FirstOrDefault(p => p.userID == usIm.userID).Image
                    });
                }
                main_frame.Visibility = Visibility.Hidden;
                ChatGroupPage();
                sendBlock.Visibility = Visibility.Visible;
                MsgTextBlock.Visibility = Visibility.Visible;
                sendMessage.Visibility = Visibility.Visible;


            }

        }

        #region Отправка сообщения 
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
        #endregion 

        #region Заполнение группового чата

        public void ChatGroupPage()
        {

            MessagesList.Clear();
            MessagesListView.Items.Clear();

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_GroupDoWork;
            worker.ProgressChanged += worker_GroupProgressChanged;
            worker.RunWorkerCompleted += worker_GroupRunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        void worker_GroupDoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            int progressPercentage = 0;
            var MessagesBD = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
            int max = MessagesBD.Count;
            foreach (var message in MessagesBD)
            {
                i++;
                progressPercentage = Convert.ToInt32(((double)i / max) * 100);
 
                //if (message.userID == im.userID)
                
                //{
                //        MessagesList.Add(new MessageList
                //        {
                //            messageID = message.messageID,
                //            userID = (int)message.userID,
                //            Message = message.Message,
                //            sendDate = (DateTime)message.sendDate,
                //            imageUser = im.imageUser
                //        });
                //}
                
                (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
            }
            e.Result = MessagesList;

        }
        public int Groupitem = 0;
        void worker_GroupProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            if (e.UserState != null)
            {
                MessagesListView.Items.Add(MessagesList[Groupitem]);
                Groupitem++;
            }

        }
        void worker_GroupRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Groupitem = 0;
            Ltimer.Interval = TimeSpan.FromSeconds(3);
            Ltimer.Tick += GroupTimer_Tick;
            Ltimer.Start();
            msgCount = MessagesList.Count();
            worker.CancelAsync();
        }

        private void GroupTimer_Tick(object sender, EventArgs e)
        {
            GroupRefreshData();
        }

        private void GroupRefreshData()
        {
            var msgCountNew = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
            if (msgCount != msgCountNew.Count())
            {
                var newMSG = msgCountNew.Last();
                foreach (var im in usersImageList)
                {
                    if (newMSG.userID == im.userID)
                    {
                        MessagesList.Add(new MessageList
                        {
                            messageID = newMSG.messageID,
                            userID = (int)newMSG.userID,
                            Message = newMSG.Message,
                            sendDate = (DateTime)newMSG.sendDate,
                            imageUser = im.imageUser
                        });
                    }
                }
                msgCount = MessagesList.Count();
                MessagesListView.Items.Add(MessagesList[msgCount - 1]);
                
                DataChanged?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        #region Заполнение персонального чата поочередно, а также обновление в реальном времени 
        
        private void PersonalMessageAdd()
        {

            MessagesList.Clear();
            MessagesListView.Items.Clear();

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_PersonalDoWork;
            worker.ProgressChanged += worker_PersonalProgressChanged;
            worker.RunWorkerCompleted += worker_PersonalRunWorkerCompleted;
            worker.RunWorkerAsync();
           
        }

        void worker_PersonalDoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            int progressPercentage = 0;
            var MessagesBD = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
            int max = MessagesBD.Count;

            foreach (var message in MessagesBD)
            {
                i++;
                progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                if (message.userID == curUserID)
                {
                    MessagesList.Add(new MessageList
                    {
                        messageID = message.messageID,
                        userID = (int)message.userID,
                        Message = message.Message,
                        sendDate = (DateTime)message.sendDate,
                        imageUser = curUsImg
                    });
                }
                else
                    MessagesList.Add(new MessageList
                    {
                        messageID = message.messageID,
                        userID = (int)message.userID,
                        Message = message.Message,
                        sendDate = (DateTime)message.sendDate,
                        imageUser = usImg
                    });
                (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
            }
            e.Result = MessagesList;

        }
        public int item = 0;
        void worker_PersonalProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            //pbCalculationProgress.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                MessagesListView.Items.Add(MessagesList[item]);
                item++;
            }

        }
        void worker_PersonalRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            item = 0;
            Ltimer.Interval = TimeSpan.FromSeconds(2);
            Ltimer.Tick += Timer_Tick;
            Ltimer.Start();
            msgCount = MessagesList.Count();
            worker.CancelAsync();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
                var msgCountNew = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
                if (msgCount != msgCountNew.Count())
                {
                    var newMSG = msgCountNew.Last();

                    if (newMSG.userID == curUserID)
                    {
                        MessagesList.Add(new MessageList
                        {
                            messageID = newMSG.messageID,
                            userID = (int)newMSG.userID,
                            Message = newMSG.Message,
                            sendDate = (DateTime)newMSG.sendDate,
                            imageUser = curUsImg
                        });
                    }
                    else
                        MessagesList.Add(new MessageList
                        {
                            messageID = newMSG.messageID,
                            userID = (int)newMSG.userID,
                            Message = newMSG.Message,
                            sendDate = (DateTime)newMSG.sendDate,
                            imageUser = usImg
                        });
                    msgCount = MessagesList.Count();
                    MessagesListView.Items.Add(MessagesList[msgCount - 1]);
                    DataChanged?.Invoke(this, new EventArgs());
                
            }
            
        }
        #endregion 
        
        #endregion
        

        private void menu_button_Click(object sender, RoutedEventArgs e)
        {
            if (menu_button.IsChecked == true)
            {
                navigation_menu_text_block.Visibility = Visibility.Visible;
                charts_text_block.Visibility = Visibility.Visible;
                tasks_text_block.Visibility = Visibility.Visible;
                users_text_block.Visibility = Visibility.Visible;
                exit_text_block.Visibility = Visibility.Visible;
                user_name_menu.Visibility = Visibility.Visible;
                user_grade.Visibility = Visibility.Visible;
            }
            else
            {
                navigation_menu_text_block.Visibility = Visibility.Collapsed;
                charts_text_block.Visibility = Visibility.Collapsed;
                tasks_text_block.Visibility = Visibility.Collapsed;
                users_text_block.Visibility = Visibility.Collapsed;
                exit_text_block.Visibility = Visibility.Collapsed;
                user_name_menu.Visibility = Visibility.Collapsed;
                user_grade.Visibility = Visibility.Collapsed;
            }
        }
    }

}
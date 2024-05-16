using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public event EventHandler DataChanged;
        public int thisChatID;
        private DispatcherTimer timer;
        int msgCount;
        public ChatPage(chats selectedChat)
        {
            InitializeComponent();
            thisChatID = selectedChat.ChatID;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            ChatPageModel model = new ChatPageModel(thisChatID);
            msgCount = model.MessagesH.Count();
            DataContext = model;
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
                ChatPageModel model = new ChatPageModel(thisChatID);
                msgCount = model.MessagesH.Count();
                DataContext = model;
                DataChanged?.Invoke(this, new EventArgs());
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
    }
}

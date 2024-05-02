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

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        List<MessagesList> messagesList = new List<MessagesList>();
        public int thisChatID;
        public ChatPage(chats selectedChat)
        {
            InitializeComponent();
            thisChatID = selectedChat.ChatID;
            Refresh();
        }

        private void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            string msg = MsgTextBlock.Text.ToString();
            var us = App.ContextDatabase.User.FirstOrDefault(t => t.userID == GetCurrent.CurrentUser.userID);
            
            if (msg != null)
            {
                var Messages = new DatabaseSQL.Messages
                {
                    chatID = thisChatID,
                    userID = us.userID,
                    Message = msg,
                    sendDate = DateTime.Now,
                };
                App.ContextDatabase.Messages.Add(Messages);
                App.ContextDatabase.SaveChanges();
                MsgTextBlock.Text = "";
                Refresh();

            }
            else
            {
                string errorMessage = "Сообщение не может быть пустым!";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.ShowDialog();
            }
            
       }
        private void Refresh()
        {
            messagesList.Clear();
            var Messages = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
            foreach (var message in Messages)
            {
                messagesList.Add(new MessagesList
                {
                    messageID = message.messageID,
                    userID = (int)message.userID,
                    Message = message.Message,
                    sendDate = (DateTime)message.sendDate,
                    imageUser = App.ContextDatabase.User.FirstOrDefault(p => p.userID == message.userID).Image
                });
            }
            MessagesListView.ItemsSource = messagesList;
        }
    }
}

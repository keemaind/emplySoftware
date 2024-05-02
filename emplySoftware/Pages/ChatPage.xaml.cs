using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
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
        List<MessagesList> MessagesList = new List<MessagesList>();
        public ChatPage(chats selectedChat)
        {
            InitializeComponent();
            ChatName.Text = selectedChat.Title;
            var Messages = App.ContextDatabase.Messages.Where(p => p.chatID == selectedChat.ChatID).ToList();
            foreach (var message in Messages)
            {
                MessagesList.Add(new MessagesList
                {
                   messageID = message.messageID,
                   userID = (int)message.userID,
                   Message = message.Message,
                   sendDate = (DateTime)message.sendDate
                });
                
            }
        }
    }
}

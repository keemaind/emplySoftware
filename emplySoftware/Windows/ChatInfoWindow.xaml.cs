using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Windows.System;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatInfoWindow.xaml
    /// </summary>
    public partial class ChatInfoWindow : Window
    {
        List<DatabaseSQL.User> usersList = new List<DatabaseSQL.User>();
        private byte[] _mainImageData;

        public ChatInfoWindow(int chatID)
        {
            InitializeComponent();
            var chat = App.ContextDatabase.chatList.Where(x => x.chatID == chatID).ToList().First();

            if (chat.userID == GetCurrent.CurrentUser.userID)
            {
                editChat.Visibility = Visibility.Visible;
                addUser.Visibility = Visibility.Visible;
            }
            ImageChat.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(chat.Image);
            ChatTitle.Text = chat.Title.ToString();
            var Users = App.ContextDatabase.chatUsers.Where(x => x.chatID == chatID).ToList();
            var UsersALL = App.ContextDatabase.User.ToList();
            foreach (var us in Users)
            {
                DatabaseSQL.User use = UsersALL.First(x => x.userID == us.userID);
                usersList.Add(use);
            }
            UsersList.ItemsSource = usersList;
            int count = Users.Count();
            usersCount.Text = $"{count} участника";
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void editChat_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using emplySoftware.Windows;
using Microsoft.Win32;


namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChatCreatePage.xaml
    /// </summary>
    public partial class ChatCreatePage : Page
    {
        public event EventHandler DataChanged;
        private byte[] _mainImageData;
        private bool Im = false;
        List<User> _users = new List<User>();
        public ChatCreatePage()
        {
            InitializeComponent();
            FillUsers();
        }
        
        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageUser.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
                Im = true;
            }
        }
        private void FillUsers()
        {
            var Users = App.ContextDatabase.User.ToList();
            foreach (var User in Users)
            {
                _users.Add(User);
            }
            UsersList.ItemsSource = _users;
        }


        private void GroupChat_Checked(object sender, RoutedEventArgs e)
        {
            BtnSelectImage.Visibility = Visibility.Visible;
            TitleChat.Visibility = Visibility.Visible;
            PersonalChat.IsChecked = false;
            UsersList.Visibility = Visibility.Visible;
            UsersList.SelectionMode = SelectionMode.Multiple;
        }

        private void PersonalChat_Checked(object sender, RoutedEventArgs e)
        {
            BtnSelectImage.Visibility = Visibility.Hidden;
            TitleChat.Visibility = Visibility.Hidden;
            GroupChat.IsChecked = false;
            UsersList.Visibility = Visibility.Visible;
            UsersList.SelectionMode = SelectionMode.Single;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            int curINT = GetCurrent.CurrentUser.userID;
            if ((bool)GroupChat.IsChecked)
            {
                DateTime nn = DateTime.Now;
                var newGroupChat = new chatList
                {
                    Title = TitleChat.Text.ToString(),
                    userID = curINT,
                    personal = false,
                    Image = _mainImageData,
                    CreateDate = nn,
                };
                App.ContextDatabase.chatList.Add(newGroupChat);
                App.ContextDatabase.SaveChanges();
                int newChatId = App.ContextDatabase.chatList.FirstOrDefault(p => p.CreateDate == nn && p.Title == TitleChat.Text.ToString()).chatID;
                foreach (User users in UsersList.SelectedItems)
                {
                    var usersInChat = new chatUsers
                    {
                        chatID = newChatId,
                        userID = users.userID
                    };

                    App.ContextDatabase.chatUsers.Add(usersInChat);
                }
                var creatorChat = new chatUsers
                {
                    chatID = newChatId,
                    userID = curINT,
                };
                App.ContextDatabase.chatUsers.Add(creatorChat);
                App.ContextDatabase.SaveChanges();
                DataChanged?.Invoke(this, new EventArgs());
            }
            else if ((bool)PersonalChat.IsChecked)
            {
                
                DateTime nn = DateTime.Now;
                var newPersonalChat = new chatList
                {
                    Title = "Личный чат",
                    userID = curINT,
                    personal = true,
                    CreateDate=nn,
                };
                App.ContextDatabase.chatList.Add(newPersonalChat);
                App.ContextDatabase.SaveChanges();

                int newChatId = App.ContextDatabase.chatList.FirstOrDefault(p => p.CreateDate == nn && p.Title == TitleChat.Text.ToString()).chatID;
                foreach (User users in UsersList.SelectedItems)
                {
                    var usersInChat = new chatUsers
                    {
                        chatID = newChatId,
                        userID = users.userID
                    };

                    App.ContextDatabase.chatUsers.Add(usersInChat);
                }
                var creatorChat = new chatUsers
                {
                    chatID= newChatId,
                    userID = curINT,
                };
                App.ContextDatabase.chatUsers.Add(creatorChat);
                App.ContextDatabase.SaveChanges();

                DataChanged?.Invoke(this, new EventArgs());
            }
           else 
            {
                string errorMessage = "Выберите тип чата";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.ShowDialog();
            }

        }
    }
}

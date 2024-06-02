using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
                if(User.userID != GetCurrent.CurrentUser.userID)
                {
                    _users.Add(User);
                }
                
            }
            UsersList.ItemsSource = _users;
        }


        private void GroupChat_Checked(object sender, RoutedEventArgs e)
        {
            BtnSelectImage.Visibility = Visibility.Visible;
            TitleChat.Visibility = Visibility.Visible;
            preview_border_contacts.Visibility = Visibility.Collapsed;
            preview_text_block_contacts.Visibility = Visibility.Collapsed;
            preview_border_group.Visibility = Visibility.Collapsed;
            preview_text_block_group.Visibility = Visibility.Collapsed;
            PersonalChat.IsChecked = false;
            UsersList.Visibility = Visibility.Visible;
            UsersList.SelectionMode = SelectionMode.Multiple;
        }

        private void PersonalChat_Checked(object sender, RoutedEventArgs e)
        {
            BtnSelectImage.Visibility = Visibility.Collapsed;
            preview_border_group.Visibility = Visibility.Visible;
            preview_border_contacts.Visibility = Visibility.Collapsed;
            preview_text_block_contacts.Visibility = Visibility.Collapsed;
            preview_text_block_group.Visibility = Visibility.Visible;
            TitleChat.Visibility = Visibility.Collapsed;
            GroupChat.IsChecked = false;
            UsersList.Visibility = Visibility.Visible;
            UsersList.SelectionMode = SelectionMode.Single;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            int curINT = GetCurrent.CurrentUser.userID;
            if ((bool)GroupChat.IsChecked)
            {
                DateTime datecreate = DateTime.Now;
                SqlDateTime sqlNow = new SqlDateTime(datecreate);
                if(_mainImageData == null)
                {

                }
                var newGroupChat = new chatList
                {
                    Title = TitleChat.Text.ToString(),
                    userID = curINT,
                    personal = false,
                    Image = _mainImageData,
                    CreateDate = datecreate,
                };
                App.ContextDatabase.chatList.Add(newGroupChat);
                App.ContextDatabase.SaveChanges();
               
                int newChat = App.ContextDatabase.chatList.FirstOrDefault(p => p.CreateDate == sqlNow.Value && p.Title == newGroupChat.Title).chatID;

                foreach (User users in UsersList.SelectedItems)
                {
                    var usersInChat = new chatUsers
                    {
                        chatID = newChat,
                        userID = users.userID
                    };

                    App.ContextDatabase.chatUsers.Add(usersInChat);
                }
                var creatorChat = new chatUsers
                {
                    chatID = newChat,
                    userID = curINT,
                };
                App.ContextDatabase.chatUsers.Add(creatorChat);
                App.ContextDatabase.SaveChanges();
                DataChanged?.Invoke(this, new EventArgs());
            }
            else if ((bool)PersonalChat.IsChecked)
            {
                DateTime nn = DateTime.Now;
                User sel = UsersList.SelectedItem as User;
                int chip = 0;
                var chtsel = App.ContextDatabase.chatList.Where(x => x.userID == sel.userID && x.personal == true).ToList();
                foreach(var jj in chtsel )
                {
                    var chatExist = App.ContextDatabase.chatUsers.FirstOrDefault(x => x.chatID == jj.chatID);
                    if (chatExist.userID == curINT)
                    {
                        string errorMessage = "Чат с данным пользователем уже существует";
                        ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                        errorWindow.ShowDialog();
                        chip = 1;
                        break;
                    }
                    
                    
                    
                }
                if (chip == 0) { 
                
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

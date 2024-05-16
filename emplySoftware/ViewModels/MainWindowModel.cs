using emplySoftware.DatabaseSQL;
using emplySoftware.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;

namespace emplySoftware.Class
{
    internal class MainWindowModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<chats> userChat = null;

        public ObservableCollection<chats> userChats
        {
            get
            {
                userChat = userChat ?? new ObservableCollection<chats>();
                return userChat;
            }
        }
        public byte[] imageUserST { get; set; }

        public List<System.Windows.Controls.Page> listChats = new List<System.Windows.Controls.Page>();

        public MainWindowModel()
        {
            FillChats(GetCurrent.CurrentUser);
            createChatPages();
            CurrentUser();
        }
        private void FillChats(User currentUser)
        {
            //Заполнение созданных чатов если он личный, заполняется ФИО с кем ведется чат 
            var chatListPersonalCreate = App.ContextDatabase.chatList.Where(p => p.userID == currentUser.userID && p.personal == true);
            foreach (var chatPers in chatListPersonalCreate)
            {
                string last;
                var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatPers.chatID).ToList();
                if (ll.Count() == 0) last = " ";
                else last = ll.Last().Message.ToString();
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
                        lastMessage = last
                    });
                }
            }
            //Заполнение чатов где пользователь в чате с кем то
            var chatListPersonalGet = App.ContextDatabase.chatUsers.Where(p => p.userID == currentUser.userID);
            foreach (var chatPersGet in chatListPersonalGet)
            {
                string last;
                var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatPersGet.chatID).ToList();
                if (ll.Count() == 0) last = " ";
                else last = ll.Last().Message.ToString();
                var personalGet = App.ContextDatabase.chatList.Where(p => p.chatID == chatPersGet.chatID && p.personal == true);
                foreach (var chatUS in personalGet)
                {
                    var us = App.ContextDatabase.User.FirstOrDefault(p => p.userID == chatUS.userID);

                    string G = FIOus.GetNotFullName(us).ToString();
                    userChats.Add(new chats
                    {
                        Title = G,
                        Image = us.Image,
                        ChatID = chatUS.chatID,
                        lastMessage = last
                    });
                }
            }
            //Заполнение созданных групповых чатов, заполняется название чата
            var chatListGroupCreate = App.ContextDatabase.chatList.Where(p => p.userID == currentUser.userID && p.personal == false);
            foreach (var chatGrop in chatListGroupCreate)
            {
                string last;
                var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatGrop.chatID).ToList();
                if (ll.Count() == 0) last = " ";
                else last = ll.Last().Message.ToString();
                userChats.Add(new chats
                {
                    Title = chatGrop.Title,
                    Image = chatGrop.Image,
                    ChatID = chatGrop.chatID,
                    lastMessage = last
                });
            }
            //Заполнение групповых чатов где находится пользователь, заполняется название чата
            var chatListGroupGet = App.ContextDatabase.chatUsers.Where(p => p.userID == currentUser.userID);
            foreach (var chatGrop in chatListGroupGet)
            {

                var chatGropGet = App.ContextDatabase.chatList.Where(p => p.chatID == chatGrop.chatID && p.personal == false);
                foreach (var chatGet in chatGropGet)
                {
                    string last;
                    var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatGet.chatID).ToList();
                    if (ll.Count() == 0) last = " ";
                    else last = ll.Last().Message.ToString();
                    userChats.Add(new chats
                    {
                        Title = chatGet.Title,
                        Image = chatGet.Image,
                        ChatID = chatGet.chatID,
                        lastMessage = last
                    });
                }
            }
        }

        private void CurrentUser()
        {
            var userImage = App.ContextDatabase.User.Where(p => p.userID == GetCurrent.CurrentUser.userID).ToList();
            foreach (var user in userImage)
            {
                imageUserST = user.Image;
            }
        }

        private void createChatPages()
        {
            //listChats.Clear();
            //foreach (chats item in userChats)
            //{
            //    ChatPage createdPage = new ChatPage(item);
            //    listChats.Add(createdPage);

            //}
        }




    }
}

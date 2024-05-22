using emplySoftware.DatabaseSQL;
using emplySoftware.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            userChats.Clear();
            //Заполнение чатов
            var chatUs = App.ContextDatabase.chatUsers.Where(p => p.userID == currentUser.userID).ToList();
            foreach (var chat in chatUs)
            {
                var chats = App.ContextDatabase.chatList.Where(p => p.chatID == chat.chatID).ToList();
                foreach(var chatj in chats)
                {
                    
                    if (chatj.personal==true)
                    {
                        string last = App.ContextDatabase.Messages.Where(p => p.chatID == chatj.chatID).ToList().Last().Message.ToString();
                        //string last = ll.Message.ToString();
                        var us = App.ContextDatabase.User.FirstOrDefault(p => p.userID == (App.ContextDatabase.chatUsers.FirstOrDefault(x => x.chatID == chatj.chatID).userID));
                        string G = FIOus.GetNotFullName(us).ToString();
                        userChats.Add(new chats
                        {
                            Title = G,
                            Image = us.Image,
                            ChatID = chatj.chatID,
                            LastMessage = last
                        });
                    }
                    else
                    {
                        string last = App.ContextDatabase.Messages.Where(p => p.chatID == chatj.chatID).ToList().Last().Message.ToString();
                        userChats.Add(new chats
                        {
                            Title = chatj.Title,
                            Image = chatj.Image,
                            ChatID = chatj.chatID,
                            LastMessage = last
                        });

                    }
                }
            }



            //foreach (var chatPers in chatListPersonalCreate)
            //{
            //    string last;
            //    var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatPers.chatID).ToList();
            //    if (ll.Count() == 0) last = " ";
            //    else last = ll.Last().Message.ToString();
            //    var personalUser = App.ContextDatabase.chatUsers.Where(p => p.chatID == chatPers.chatID);
            //    foreach (var chatUS in personalUser)
            //    {
            //        var us = App.ContextDatabase.User.FirstOrDefault(p => p.userID == chatUS.userID);
            //        string G = FIOus.GetNotFullName(us).ToString();

            //        userChats.Add(new chats
            //        {
            //            Title = G,
            //            Image = us.Image,
            //            ChatID = chatPers.chatID,
            //            LastMessage = last
            //        });
            //    }
            //}
            ////Заполнение чатов где пользователь в чате с кем то
            //var chatListPersonalGet = App.ContextDatabase.chatUsers.Where(p => p.userID == currentUser.userID);
            //foreach (var chatPersGet in chatListPersonalGet)
            //{
            //    string last;
            //    var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatPersGet.chatID).ToList();
            //    if (ll.Count() == 0) last = " ";
            //    else last = ll.Last().Message.ToString();
            //    var personalGet = App.ContextDatabase.chatList.Where(p => p.chatID == chatPersGet.chatID && p.personal == true);
            //    foreach (var chatUS in personalGet)
            //    {
            //        var us = App.ContextDatabase.User.FirstOrDefault(p => p.userID == chatUS.userID);

            //        string G = FIOus.GetNotFullName(us).ToString();
            //        userChats.Add(new chats
            //        {
            //            Title = G,
            //            Image = us.Image,
            //            ChatID = chatUS.chatID,
            //            LastMessage = last
            //        });
            //    }
            //}
            ////Заполнение созданных групповых чатов, заполняется название чата
            //var chatListGroupCreate = App.ContextDatabase.chatList.Where(p => p.userID == currentUser.userID && p.personal == false);
            //foreach (var chatGrop in chatListGroupCreate)
            //{
            //    string last;
            //    var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatGrop.chatID).ToList();
            //    if (ll.Count() == 0) last = " ";
            //    else last = ll.Last().Message.ToString();
            //    userChats.Add(new chats
            //    {
            //        Title = chatGrop.Title,
            //        Image = chatGrop.Image,
            //        ChatID = chatGrop.chatID,
            //        LastMessage = last
            //    });
            //}
            ////Заполнение групповых чатов где находится пользователь, заполняется название чата
            //var chatListGroupGet = App.ContextDatabase.chatUsers.Where(p => p.userID == currentUser.userID);
            //foreach (var chatGrop in chatListGroupGet)
            //{

            //    var chatGropGet = App.ContextDatabase.chatList.Where(p => p.chatID == chatGrop.chatID && p.personal == false);
            //    foreach (var chatGet in chatGropGet)
            //    {
            //        string last;
            //        var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatGet.chatID).ToList();
            //        if (ll.Count() == 0) last = " ";
            //        else last = ll.Last().Message.ToString();
            //        userChats.Add(new chats
            //        {
            //            Title = chatGet.Title,
            //            Image = chatGet.Image,
            //            ChatID = chatGet.chatID,
            //            LastMessage = last
            //        });
            //    }
            //}
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

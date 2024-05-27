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
                        string last;
                        var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatj.chatID).ToList();
                        if (ll.Count() == 0)
                        {
                             last = "";
                        }
                        else last = ll.Last().Message.ToString();
                        
                       
                        var chatUsers = App.ContextDatabase.chatUsers.Where(x => x.chatID == chatj.chatID ).ToList();
                        foreach(var uss in chatUsers)
                        {
                            if(uss.userID == GetCurrent.CurrentUser.userID)
                            {}
                            else
                            {
                                var us = App.ContextDatabase.User.FirstOrDefault(p => p.userID == uss.userID);
                                string G = FIOus.GetNotFullName(us).ToString();
                                userChats.Add(new chats
                                {
                                    Title = G,
                                    Image = us.Image,
                                    ChatID = chatj.chatID,
                                    LastMessage = last
                                });
                            }
                        }
                    }
                    else
                    {
                        string last;
                        var ll = App.ContextDatabase.Messages.Where(p => p.chatID == chatj.chatID).ToList();
                        if (ll.Count() == 0)
                        {
                            last = "";
                        }
                        else last = ll.Last().Message.ToString();
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

        }

        



        private void CurrentUser()
        {
            imageUserST = GetCurrent.CurrentUser.Image;
            
        }

        




    }
}

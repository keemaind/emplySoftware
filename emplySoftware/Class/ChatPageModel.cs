using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Input;


namespace emplySoftware.Class
{
    public class ChatPageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Messages> messages = null;


        public ObservableCollection<Messages> MessagesH
        {
            get
            {
                messages = messages ?? new ObservableCollection<Messages>();
                return messages;
            }
        }

        public ChatPageModel(int thisChatID)
        {
            Refresh(thisChatID);
        }
        private void Refresh(int thisChatID)
        {
            
            var MessagesBD = App.ContextDatabase.Messages.Where(p => p.chatID == thisChatID).ToList();
            foreach (var message in MessagesBD)
            {
                MessagesH.Add(new Messages
                {
                    messageID = message.messageID,
                    userID = (int)message.userID,
                    Message = message.Message,
                    sendDate = (DateTime)message.sendDate,
                    imageUser = App.ContextDatabase.User.FirstOrDefault(p => p.userID == message.userID).Image
                });
            }
        }



    }
}

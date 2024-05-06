using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class chats
    {
        string title;
        int chatID;
        byte[] image;
        public string lastMessage;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public int ChatID
        {
            get { return chatID; }
            set { chatID = value; }
        }
        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }
        public string LastMessage
        {
            get { return lastMessage; }
            set { lastMessage = value; }
        }
        
    }
}

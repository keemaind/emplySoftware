using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class MessagesList : Messages
    {
        public int messageID { get; set; }
        public int chatID { get; set; }
        public int userID { get; set; }
        public string Message { get; set; }
        public System.DateTime sendDate { get; set; }

    }
}

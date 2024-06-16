using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class MessageList
    {
        public int messageID { get; set; }
        public int chatID { get; set; }
        public int userID { get; set; }
        public string userFIO { get; set; }
        public string Message { get; set; }
        public DateTime sendDate { get; set; }

        public virtual chatList chatList { get; set; }
        public virtual User User { get; set; }
        public byte[] imageUser { get; set; }
    }
}

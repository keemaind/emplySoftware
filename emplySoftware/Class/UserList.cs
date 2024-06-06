using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class UserList
    {
        string fioUser;

        public string FioUser
        {
            get { return fioUser; }
            set { fioUser = value; }
        }
        public int userID { get; set; }
        public byte[] imageUser { get; set; }
        public string Position { get; set; }
        public string Login { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
        public DateTime StartSession { get; set; }
    }
}

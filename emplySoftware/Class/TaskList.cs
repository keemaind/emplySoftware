using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class TaskList
    {
        string fioUser;

        public string FioUser
        {
            get { return fioUser; }
            set { fioUser = value; }
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public Nullable<double> Difficulty { get; set; }
        public string Status { get; set; }
        
    }
}

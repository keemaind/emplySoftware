using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    internal class GetCurrent : DatabaseSQL.User
    {
        public static User CurrentUser { get; set; }
        public static bool Admin {  get; set; }
    }
}

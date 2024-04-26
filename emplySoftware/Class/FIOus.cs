using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class FIOus
    {
        public static string GetFullName2(User user)
        {
            return $"{user.MiddleName} {user.FirstName[0]}. {user.LastName[0]}.";
        }
        public static string GetFullName(User user)
        {
            return user.MiddleName + " " + user.FirstName + " " + user.LastName;
        }

        public static string GetNotFullName(User user)
        {
            return user.MiddleName + " " + user.FirstName;
        }

    }
}

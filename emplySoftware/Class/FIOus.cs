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

        public static string GetNotFullNameInt(int? employeeID)
        {
            User us = App.ContextDatabase.User.Where(p => p.userID == employeeID).FirstOrDefault();
            return $"{us.MiddleName} {us.FirstName[0]}. {us.LastName[0]}.";
        }
        public static string GetFullNameInt(int? employeeID)
        {
            User us = App.ContextDatabase.User.Where(p => p.userID == employeeID).FirstOrDefault();
            return us.MiddleName + " " + us.FirstName + " " + us.LastName;
        }
    }
}

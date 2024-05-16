using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class tasks : DatabaseSQL.Task
    {
        string fioUser;

        public string FioUser
        {
            get { return fioUser; }
            private set { fioUser = value; }
        }


        public static List<tasks> TaskFills(User user)
        {

            List<DatabaseSQL.Task> tasks = App.ContextDatabase.Task.Where(x => x.EmployeeID == user.userID).ToList();
            List<tasks> taskFills = new List<tasks>();
            for (int i = 0; i < tasks.Count; i++)
            {
                taskFills.Add(new tasks
                {
                    ID = tasks[i].ID,
                    EmployeeID = tasks[i].EmployeeID,
                    Title = tasks[i].Title,
                    Description = tasks[i].Description,
                    CreateDate = tasks[i].CreateDate,
                    Deadline = tasks[i].Deadline,
                    Difficulty = tasks[i].Difficulty,
                    Status = tasks[i].Status,
                    FioUser = FIOus.GetFullName(tasks[i].EmployeeID),
                });
            }
            return taskFills.ToList();
        }

       
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace emplySoftware.DatabaseSQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public Nullable<double> Difficulty { get; set; }
        public string Status { get; set; }
        string fioUser;

        public string FioUser
        {
            get { return fioUser; }
            set { fioUser = value; }
        }

        public virtual User User { get; set; }
    }
}

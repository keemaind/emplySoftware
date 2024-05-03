using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emplySoftware.Class
{
    public class SQLNotifier : IDisposable
    {
        public SqlCommand CurrentCommand { get; set; }
        private SqlConnection connection;
        public SqlConnection CurrentConnection
        {
            get
            {
                connection = connection ??
            new SqlConnection(ConnectionString);
                return connection;
            }
        }
        public string ConnectionString
        {
            get
            {
                return @"Data Source=VPMT.RU\IS4;initial catalog=emplyDatabase;user id=user27;password=user27;encrypt=False;MultipleActiveResultSets=True;App=EntityFramework";

            }
        }

        public SQLNotifier()
        {
            SqlDependency.Start(ConnectionString);

        }
        private event EventHandler<SqlNotificationEventArgs> _newMessage;

        public event EventHandler<SqlNotificationEventArgs> NewMessage
        {
            add
            {
                _newMessage += value;
            }
            remove
            {
                _newMessage -= value;
            }
        }

        public virtual void OnNewMessage(SqlNotificationEventArgs notification)
        {
            if (_newMessage != null)
                _newMessage(this, notification);
        }
        public DataTable RegisterDependency()
        {

            CurrentCommand = new SqlCommand("Select [messageID],[chatID],[userID],[Message],[sendDate] from dbo.Messages", CurrentConnection);

            CurrentCommand.Notification = null;

            SqlDependency dependency = new SqlDependency(CurrentCommand);
            dependency.OnChange += dependency_OnChange;

            if (CurrentConnection.State == ConnectionState.Closed)
                CurrentConnection.Open();
            try
            {

                DataTable dt = new DataTable();
                dt.Load(CurrentCommand.ExecuteReader
            (CommandBehavior.CloseConnection));
                return dt;
            }
            catch { return null; }
        }

        void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;

            dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

            OnNewMessage(e);
        }

        public void Dispose()
        {
            SqlDependency.Stop(ConnectionString);
        }
    }
}

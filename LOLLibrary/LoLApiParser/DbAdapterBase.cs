using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace LoLApiParser
{
    public abstract class DbAdapterBase : IDisposable
    {
        private readonly object sync = new object();
        private readonly string dbHost;
        private readonly string user;
        private readonly string pass;
        private readonly string dbName;

        private MySqlConnection connection;
        public MySqlConnection Connection
        {
            get
            {
                lock (sync)
                {
                    if (connection == null)
                        connection = new MySqlConnection(
                            $"server={dbHost};User Id={user};password={pass};Allow User Variables=True;Persist Security Info=True;database={dbName};Minimum Pool Size=3;Maximum Pool Size=10;");

                    if (connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed)
                        OpenConnection();

                    return connection;
                }
            }
        }

        protected DbAdapterBase(string dbH, string dbN, string u, string p)
        {
            dbHost = dbH;
            dbName = dbN;
            user = u;
            pass = p;

            OpenConnection();
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        break;

                    case 1045:
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection?.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        public void Dispose()
        {
            CloseConnection();
            connection?.Dispose();
        }
    }

    public class LoLDbAdapter : DbAdapterBase
    {
        public LoLDbAdapter(string u, string p) : base("", "", u, p) { }
    }
}

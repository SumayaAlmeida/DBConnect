using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BowmanCarHire
{
    class DBConnection
    {
        MySqlConnection connection = null;

        string connectionString;

        public MySqlConnection Connection { get => connection; }


        public DBConnection(string servername, string databaseName, string userID, string password)
        {
            //Example Connection string
            //SERVERNAME = localhost, DATABASE = hire; USERNAME = csharp;  PASSWORD = password

            this.connectionString = string.Format($"SERVER={servername}; DATABASE = {databaseName}; UID = {userID}; PASSWORD={password};");
        }

        public bool Connect()
        {
            bool succeeded = true;

            try
            {
                this.connection = new MySqlConnection(this.connectionString);

                this.connection.Open();
            }
            catch (MySqlException ex)
            {
                succeeded = false;

                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Authentication error - please check your login credentials.");
                        break;

                    case 1045:
                        MessageBox.Show("Cannot connect to server.");
                        break;

                    default:
                        MessageBox.Show("Exception found: " + ex.Message);
                        break;
                }
            }

            return succeeded;
        }

        public bool Close()
        {
            this.connection.Close();

            return true;
        }
    }
}

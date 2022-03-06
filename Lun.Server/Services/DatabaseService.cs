using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Services
{
    class DatabaseService
    {       
        const string Host = "localhost";
        const string Database = "lun";
        const string User = "root";
        const string Password = "156156";

        // TABLES
        public const string TABLE_ACCOUNTS = "accounts";
        public const string TABLE_CHARACTERS = "characters";

        public static MySqlConnection Connection;

        public static void Initialize()
        {
            var textConnection = $"server={Host};port=3306;user id={User};pwd={Password};";
            Connection = new MySqlConnection(textConnection);
            Connection.Open();

            CheckDatabase();
            CheckTables();
        }

        public static void Close()
            => Connection?.Close();

        static void CheckDatabase()
        {
            ExecuteNonQuery($"CREATE DATABASE IF NOT EXISTS {Database};");
            ExecuteNonQuery($"USE {Database};");
        }

        static void CheckTables()
        {
            // Accounts
            ExecuteNonQuery($@"CREATE TABLE IF NOT EXISTS {TABLE_ACCOUNTS}(
id int NOT NULL AUTO_INCREMENT,
name text,
password text,
PRIMARY KEY (id)
);");

            // Characters
            ExecuteNonQuery($@"CREATE TABLE IF NOT EXISTS {TABLE_CHARACTERS}(
id int NOT NULL AUTO_INCREMENT,
accountid int NOT NULL,
slotid int NOT NULL,
name text,
classid int,
spriteid int,
x float,
y float,
PRIMARY KEY (id),
FOREIGN KEY (accountid) REFERENCES accounts(id)
);");
        }

        public static int ExecuteNonQuery(string str)
        {
            if (Connection == null)
                return -1;

            var cmd = Connection.CreateCommand();
            cmd.CommandText = str;

            return cmd.ExecuteNonQuery();            
        }

        public static MySqlDataReader ExecuteReader(string str)
        {
            if (Connection == null)
                return null;

            var cmd = Connection.CreateCommand();
            cmd.CommandText = str;

            return cmd.ExecuteReader();
        }

        public static object ExecuteScalar(string str)
        {
            if (Connection == null)
                return null;

            var cmd = Connection.CreateCommand();
            cmd.CommandText = str;

            return cmd.ExecuteScalar();
        }

        public static bool CheckDataExists(string table, string whereEntry, string whereValue)
        {
            var value = Convert.ToInt32(ExecuteScalar($"SELECT COUNT(*) FROM {table} WHERE BINARY {whereEntry}='{whereValue}';"));
            return value > 0;

        }

    }
}

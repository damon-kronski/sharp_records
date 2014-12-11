using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Active_Records
{
    public static class SQL
    {
        private static SqlConnection connection;
        private static SqlCommand command;
        private static SqlDataReader reader;

        /// <summary>
        /// Open connection to Database
        /// </summary>
        /// <param name="path">Path to mdf file</param>
        /// <param name="dataSource">source/instance name</param>
        public static void connect(string path = "", string dataSource = "(LocalDB)\\SQLEXPRESS")
        {
            // Set path if empty
            if (String.IsNullOrWhiteSpace(path))
                path = Environment.CurrentDirectory + "\\data\\database.mdf";

            // Create Command and ConnectionString
            command = new SqlCommand();
            string connstr = "Data Source=" + dataSource + ";AttachDbFilename=\"" + path + "\";Integrated Security=True;MultipleActiveResultSets=True";

            // Open connection and set to Command
            connection = new SqlConnection(connstr);
            connection.Open();
            command.Connection = connection;

        }

        /// <summary>
        /// Close connection
        /// </summary>
        public static void close()
        {
            
            try
            {
                // Try to close reader if open
                if (!reader.IsClosed) 
                    reader.Close();
            }
            catch { }

            // Close Connection if open
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        /// <summary>
        /// Run a select command and get dictionary-list
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> select(string sql)
        {
            // Create list
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();

            // Set command
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;

            // Read
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Create Dictionary
                Dictionary<string, object> lst = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // Add column-items
                    lst.Add(reader.GetName(i), reader[i]);
                }

                // Add dictionary to list
                res.Add(lst);
            }

            // Close
            reader.Close();

            return res;
        }

        /// <summary>
        /// Run a sql command and return a single field
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <returns>single field</returns>
        public static object selectField(string sql)
        {
            // Set command
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;

            // Read
            reader = command.ExecuteReader();
            reader.Read();

            object value = reader[0];

            // Close
            reader.Close();

            return value;
        }

        /// <summary>
        /// Run a sql command and check if exist
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <returns>exist bool</returns>
        public static bool exists(string sql)
        {
            // Set Command
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;

            // Read
            reader = command.ExecuteReader();
            bool res = reader.HasRows;

            // Close
            reader.Close();

            return res;
        }

        /// <summary>
        /// Run a sql command without return
        /// </summary>
        /// <param name="sql">sql command</param>
        public static void run(string sql)
        {
            // Set command
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;

            // Run
            command.ExecuteNonQuery();
        }

    }
}

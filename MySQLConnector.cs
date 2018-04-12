using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace quotingdojo.Connectors
{
    public class MySQLConnector
    {
        static string server = " localhost";

        // make sure to name your database name from mysql in the below line.
        static string db =  "myDB";
        static string port = "3306";
        static string user = "root";
        static string pass = "root";
        internal static IDbConnection Connection {
            get
            {
                return new MySqlConnection($"Server={server};Port={port};Database={db};UserID={user};Password={pass};SslMode=None");
            }
        }

        //This method runs a query and stores the response in a list of dictionary records
        public static List<Dictionary<string, object>> Query(string queryString)
        {
            using (IDbConnection MySQLConnector = Connection)
            {
                using (IDbCommand command = MySQLConnector.CreateCommand())
                {
                    command.CommandText = queryString;
                    MySQLConnector.Open();
                    var result = new List<Dictionary<string, object>>();
                    using (IDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var dict = new Dictionary<string, object>();
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                dict.Add(rdr.GetName(i), rdr.GetValue(i));
                            }
                            result.Add(dict);
                        }
                        return result;
                    }
                }
            }
        }
        //This method run a query and returns no values
        public static void Execute(string queryString)
        {
            using (IDbConnection MySQLConnector = Connection)
            {
                using (IDbCommand command = MySQLConnector.CreateCommand())
                {
                    command.CommandText = queryString;
                    MySQLConnector.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
using System;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

namespace OrderByKioskWebAPI
{
  public class DataBase
    {
        private MySqlConnection connection;
        private bool status;

        public DataBase()
        {
            status = Connection();
        }

        private bool Connection()
        {
           string host = "192.168.3.8";
            string user = "root";
            string password = "1234";
            string db = "obk";

            string connStr = string.Format(@"server={0};user={1};password={2};database={3}", host, user, password, db);
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                this.connection = conn;
                //MessageBox.Show("MS-SQL 연결 성공!");
                return true;
            }
            catch
            {
                conn.Close();
                this.connection = null;
                //MessageBox.Show("MS-SQL 연결 실패!");
                return false;
            }
        }

        public void Close()
        {
            if (status)
            {
                connection.Close();
            }
        }

        public MySqlDataReader Reader(string sql)
        {
            if (status)
            {
                try
                {
                    MySqlCommand comm = new MySqlCommand();
                    comm.CommandText = sql;
                    comm.Connection = connection;
                    comm.CommandType = CommandType.StoredProcedure;
                    return comm.ExecuteReader();
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public MySqlDataReader Reader(string sql,Hashtable hashtable)
        {
            if (status)
            {
                try
                {
                    MySqlCommand comm = new MySqlCommand();
                    comm.CommandText = sql;
                    comm.Connection = connection;
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (DictionaryEntry data in hashtable)
                    {
                        comm.Parameters.AddWithValue(data.Key.ToString(), data.Value);
                    }

                    return comm.ExecuteReader();
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool ReaderClose(MySqlDataReader sdr)
        {
            try
            {
                sdr.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool NonQuery(string sql, Hashtable ht)
        {
            if (status)
            {
                try
                {
                    MySqlCommand comm = new MySqlCommand();
                    comm.CommandText = sql;
                    comm.Connection = connection;
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (DictionaryEntry data in ht)
                    {
                        comm.Parameters.AddWithValue(data.Key.ToString(), data.Value);
                    }
                    int cnt = comm.ExecuteNonQuery();
                    Console.WriteLine("------------------>>>>>>>>"+cnt);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
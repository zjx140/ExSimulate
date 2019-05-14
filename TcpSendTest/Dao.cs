using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpSendTest
{
    class Dao
    {
        private static MySqlConnection GetConnection(MySqlConnection conn)
        {
            conn = new MySqlConnection("Host=localhost;Database=weather;Username=root;password=zjx140");
            return conn;
        }
        private static void Open(MySqlConnection conn)
        {
            conn.Open();
            Console.WriteLine("已打开");
        }
        private static void Close(MySqlConnection conn)
        {
            conn.Close();
            Console.WriteLine("已关闭");
        }
        public static List<Message> SelectData()
        {
            MySqlConnection conn = null;
            conn = GetConnection(conn);
            Open(conn);
            List<Message> list = new List<Message>();
            string sql = "select * from climate";
            MySqlCommand sqlText = new MySqlCommand(sql, conn);
            MySqlDataReader reader = sqlText.ExecuteReader();
            while (reader.Read())
            {
                //要改的
                Message message = new Message();
                message.Hour = reader.GetInt32("hours");
                message.Temp = reader.GetDouble("temp");
                message.Apptemp = reader.GetDouble("apptemp");
                message.Wind = reader.GetDouble("wind");
                message.Visible = reader.GetDouble("visible");
                message.Press = reader.GetDouble("press");
                message.Humi = reader.GetDouble("humi");
                message.Windbear = reader.GetInt16("windbear");
                list.Add(message);
            }
            /* *
             * */
            reader.Close();
            Close(conn);
            return list;
        }
    }
}

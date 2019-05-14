using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace computing
{
    class Dao
    {
        public static MySqlConnection GetConnection(MySqlConnection conn)
        {
            conn = new MySqlConnection("Host=localhost;Database=weather;Username=root;password=zjx140");
            return conn;
        }
        public static void Open(MySqlConnection conn)
        {
            conn.Open();
            Console.WriteLine("已打开");
        }
        public static void Close(MySqlConnection conn)
        {
            conn.Close();
            Console.WriteLine("已关闭");
        }

        public static List<Message> SelectData()//获取全部数据
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

        public static void Delete()
        {
            MySqlConnection conn = null;
            conn = GetConnection(conn);
            Open(conn);
            string sql = "delete from questiondata";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            Close(conn);
        }

        public static void InsertQuestionData(Message m)
        {
            MySqlConnection conn = null;
            conn = GetConnection(conn);
            Open(conn);
            string sql = "insert into questiondata(hour,temp,apptemp,humi,wind,visible,press,windbear) VALUES('" + m.Hour + "','" + m.Temp + "','" + m.Apptemp + "','" + m.Humi + "','" + m.Wind + "','" + m.Visible + "','" + m.Press + "','" + m.Windbear + "')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            Close(conn);
        }

        public static DataTable GetDataBaseTable()
        {
            string sql = "select * from questiondata";
            DataTable dataTable = new DataTable();
            MySqlConnection conn = null;
            conn = GetConnection(conn);
            Open(conn);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sql, conn);
            mySqlDataAdapter.Fill(dataTable);
            Close(conn);
            return dataTable;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace computing
{
    class TcpSend
    {
        public static void SendNormalSignal()
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.NoDelay = true;
            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 9998);
            NetworkStream network = tcpClient.GetStream();
            if (network.CanWrite)
            {
                Byte[] bytSend = Encoding.UTF8.GetBytes("正常");
                network.Write(bytSend, 0, bytSend.Length);
                network.Flush();
            }
            network.Close();
            tcpClient.Close();
        }
    }
}

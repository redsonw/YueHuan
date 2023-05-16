using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace YueHuan.Systemaitc
{
    public class Sockets
    {
        #region 获取操作系统已用的端口号
        /// <summary>
        /// 获取操作系统已用的端口号
        /// </summary>
        /// <returns>返回TCP和UDP列表。</returns>
        public static IList GetUsePort()
        {
            //获取本地计算机的网络连接和通信统计数据的信息
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            IList allPorts = new ArrayList();
            foreach (IPEndPoint ep in ipsTCP) allPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUDP) allPorts.Add(ep.Port);
            foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

            return allPorts;
        }
        #endregion

        #region 检测端口是否被占用
        /// <summary>
        /// 检测端口是否占用
        /// </summary>
        /// <param name="Port"> 端口Port </param>
        /// <returns>默认为false，端口被占用返回true。</returns>
        public static bool CheckUsePort(int Port)
        {
            bool IsUse = false;
            IList UsePort = GetUsePort();

            foreach (int p in UsePort)
            {
                if (p == Port)
                {
                    IsUse = true;
                }
            }
            return IsUse;

        }

        /// <summary>
        /// 检测端口是否被占用
        /// </summary>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool CheckPort(int Port)
        {
            bool IsUse = false;

            IPGlobalProperties IPGlobal = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] IPEndPoints = IPGlobal.GetActiveTcpListeners(); // 获取TCP端口

            foreach (IPEndPoint endPoint in IPEndPoints)
            {
                if (endPoint.Port == Port)
                {
                    IsUse = true;
                }
            }
            IPEndPoints = IPGlobal.GetActiveUdpListeners();// 获取UDP端口

            foreach (IPEndPoint EndPoint in IPEndPoints)
            {
                if (EndPoint.Port == Port)
                {
                    IsUse = true;
                }
            }
            return IsUse;
        }
        #endregion

        #region 套接字检测端口是否占用
        /// <summary>
        /// 套接字检测端口是否占用
        /// </summary>
        /// <param name="Ip">IP</param>
        /// <param name="Port">端口</param>
        /// <returns>端口失败返回false，端口被占用则返回true。</returns>
        public static bool CheckSocket(string Ip, int Port)
        {
            Socket? sock = null;
            try
            {
                IPAddress ipa = IPAddress.Parse(Ip);
                IPEndPoint point = new(ipa, Port);
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(point);
                return true;
            }
            catch // (SocketException ex)
            {
                // MessageBox.Show("计算机端口检测失败，错误消息为：" + ex.Message);
                return false;
            }
            finally
            {
                if (sock != null)
                {
                    sock.Close();
                    sock.Dispose();
                }
            }
        }
        #endregion
    }
}

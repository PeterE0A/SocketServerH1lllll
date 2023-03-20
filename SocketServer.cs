using System.Net;
using System.Net.Sockets;

namespace SocketServerH1
{
    internal class SocketServer
    {
        public SocketServer()
        {
            IPEndPoint endpoint = GetServerIp();
            StartServer(endpoint);
        }

        private void StartServer(IPEndPoint endpoint)
        {

        }

        private IPEndPoint GetServerIp()
        {
            //Gets the hostname of the machine
            string strHostName = Dns.GetHostName();
            //Uses hostnape to get host entry
            IPHostEntry host = Dns.GetHostEntry(strHostName);
            //Host entry contains all IP addressses
            //We create a list of IPv4 addresses
            List<IPAddress> addrList = new();
            int counter = 0;
            foreach (var item in host.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine($"{counter++} {item.ToString()}");
                    addrList.Add(item);
                }
            }
            //Selects the IP from the list
            int temp = 0;
            do { Console.Write("Select server IP: "); }
            while (!int.TryParse(Console.ReadLine(), out temp));

            IPAddress addr = addrList[temp];
            IPEndPoint localEndPoint = new IPEndPoint(addr, 11000);
            return localEndPoint;
        }
    }
}

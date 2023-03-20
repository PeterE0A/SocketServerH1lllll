using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServerH1
{
    internal class SocketServer
    {
        public SocketServer()
        {
            //Endpoint consists of an IP address AND a port.
            IPEndPoint endpoint = GetServerEndpoint();
            //Start server with endpoint previously selected.

            Socket listener = new(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endpoint);

            StartServer(listener);
        }

        private void StartServer(Socket listener)
        {
            listener.Listen(10);
            Console.WriteLine($"Server Listening on: {listener.LocalEndPoint}");

            Socket handler = listener.Accept();

            Console.WriteLine($"Accepting connection from {handler.RemoteEndPoint}");

            string msg = null;
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRec = handler.Receive(buffer);
                msg += Encoding.ASCII.GetString(buffer, 0, bytesRec);
                if (msg.IndexOf("<EOM>") > -1) break;
            }
            Console.WriteLine($"Message: {msg}");


        }

        private IPEndPoint GetServerEndpoint()
        {
            //Gets the hostname of the machine
            string strHostName = Dns.GetHostName();
            //Uses hostnape to get host entry
            IPHostEntry host = Dns.GetHostEntry(strHostName);
            //Host entry contains all IP addressses
            //We create a list of IPv4 addresses
            List<IPAddress> addrList = new();
            int counter = 0;
            //Loops through host addresslist and adds to our list
            //if not IPv6
            foreach (var item in host.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetworkV6) continue;
                Console.WriteLine($"{counter++} {item.ToString()}");
                addrList.Add(item);

            }
            //Selects the IP from the list
            int temp = 0;
            do { Console.Write("Select server IP: "); }
            while (!int.TryParse(Console.ReadLine(), out temp)
            || temp > addrList.Count
            || temp < 0);

            IPAddress addr = addrList[temp];
            IPEndPoint localEndPoint = new IPEndPoint(addr, 11000);
            return localEndPoint;
        }
    }
}

namespace SocketServerH1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(11000);
            server.Start();
        }
    }
}
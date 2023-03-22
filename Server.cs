using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServerH1
{
    class Server
    {
        private TcpListener listener;
        private bool gameOver = false;

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine($"Server started on {listener.LocalEndpoint}");

            var client = listener.AcceptTcpClient();
            Console.WriteLine($"Client connected from {client.Client.RemoteEndPoint}");

            var clientHandler = new ClientHandler(client);
            clientHandler.START();

            Console.WriteLine("Starting game...");

            while (!gameOver)
            {
                // Wait for client move
            }
        }

    }

    class ClientHandler
    {
        private TcpClient client;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
        }

        public void START()
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);

            writer.WriteLine("Welcome to Rock-Paper-Scissors! Please enter your move (rock, paper, or scissors)");
            writer.Flush();

            while (true)
            {
                var move = reader.ReadLine();
                Console.WriteLine($"Received move from {client.Client.RemoteEndPoint}: {move}");

                var computerMove = GetComputerMove();
                Console.WriteLine($"Computer played {computerMove}");

                var result = DetermineResult(move, computerMove);

                writer.WriteLine($"You played {move}");
                writer.WriteLine($"Computer played {computerMove}");
                writer.WriteLine(result);
                writer.Flush();
            }
        }

        private string GetComputerMove()
        {
            var moves = new List<string> { "rock", "paper", "scissors" };
            var random = new Random();
            return moves[random.Next(moves.Count)];
        }

        private string DetermineResult(string move1, string move2)
        {
            if (move1 == move2)
            {
                return "It's a tie!";
            }

            if ((move1 == "rock" && move2 == "scissors") ||
                (move1 == "scissors" && move2 == "paper") ||
                (move1 == "paper" && move2 == "rock"))
            {
                return "You win!";
            }

            return "Computer wins!";
        }
    }
}

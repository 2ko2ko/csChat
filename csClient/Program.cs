using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketChatClient
{
    public static class  Program
    {
        const int defaultPort = 8888; // Default Server port
        const string defaultIp = "127.0.0.1"; // Default IP
        const int bufferSize = 1024; // Buffer size
        
        static TcpClient client;
        public static NetworkStream stream;

        static void Main(string[] args)
        {
            string ip = defaultIp;
            int port = defaultPort;
            if (args.Length > 0)
            {
                if (args[0] == "h" || args[0] == "help") //if h or help show help instead of connecting to help
                {
                    Console.WriteLine($"Usage: \ncsClient [ip] [port] \n");
                    Environment.Exit(0);
                }
                ip = args[0];
            }
            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out port))
                {
                    Console.WriteLine("Invalid Port, using Default Port: 8888");
                    port = defaultPort;
                }
                //port = args[1]; // int != string... rip
            }
            
            //try
            //{
                client = new TcpClient (ip, port); 
                stream = client.GetStream();

                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine("Connected to the chat server.");
                //Console.WriteLine("Enter Username: ");
                Thread receiveThread = new Thread(ReceiveMessages); // Thread to receive messages
                receiveThread.Start();
                SendMessages(); // Send messages in main thread
                client.Close(); // Close client
                Console.WriteLine("Disconnected from the chat server.");
            /*}
            catch (Exception conexc)
            {
                Console.WriteLine($"Connetion failed: {conexc.Message}");
                Environment.Exit(1);
            }*/
        }

        static void ReceiveMessages()
        {
            byte[] buffer = new byte[bufferSize]; // Create buffer
            StringBuilder message = new StringBuilder(); // Create message

            while (true)
            {
                try
                {
                    int bytesRead = 0; // Read from stream until \n
                    do
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        message.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    }
                    while (stream.DataAvailable);

                    if (message.Length == 0) // Server disconnected
                    {
                        break;
                    }

                    message.Length--; // Remove \n from message

                    Console.WriteLine(message); // Print message

                    message.Clear(); // Clear message for next read
                }
                catch (Exception ex) // Error
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    break;
                }
            }
        }

        static void SendMessages()
        {
            /*while (true)
            {
                try
                {
                    string input = Console.ReadLine(); // Read input from console

                    if (input == "exit") // Exit command
                    {
                        break;
                    }

                    input += "\n"; // Add \n to input and convert to bytes
                    byte[] buffer = Encoding.UTF8.GetBytes(input);

                    stream.Write(buffer, 0, buffer.Length); // Write and flush buffer
                    stream.Flush();
                }
                catch (Exception ex) // Error
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    break;
                }
            }*/
            //InputeHandler ihandle = new InputeHandler(stream);
            var ihandle = new InputeHandler(stream);
            ihandle.Run();
        }
    }

    public class InputeHandler
    {
        private readonly List<string> history = new List<string>();
        private int historyIndex = -1;
        private string currentIndex = "";
        private NetworkStream _stream;

        private static Dictionary<ConsoleKey, Action> keyActions = new Dictionary<ConsoleKey, Action>();

        public InputeHandler(NetworkStream stream)
        //public InputeHandler()
        {
            _stream = stream;
            //private NetworkStream _stream = Program;
            
            keyActions[ConsoleKey.Enter] = OnEnter;
            keyActions[ConsoleKey.Backspace] = OnBackspace;
            keyActions[ConsoleKey.UpArrow] = OnUpArrow;
            keyActions[ConsoleKey.DownArrow] = OnDownArrow;
            keyActions[ConsoleKey.Escape] = OnEscape;

        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    char keyChar = keyInfo.KeyChar;
    
                    if (keyActions.TryGetValue(keyInfo.Key, out Action? action))
                    {
                        action();
                    }
                    else if (!char.IsControl(keyChar))
                    {
                        currentIndex += keyChar;
                        Console.Write(keyChar);
                    }
                }
                catch (Exception exrun)
                {
                    Console.WriteLine($"Error: {exrun.Message}");
                    break;
                }

            }
        }

        private void RedrawLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth -1));
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(currentIndex);
        }

        private void OnEnter()
        {
            if(!string.IsNullOrWhiteSpace(currentIndex)) //Checks if buffered input is NOT empty, space or NULL
            {
                if(history.Count == 0 || history[^1] != currentIndex)
                {
                    history.Add(currentIndex);
                }
                historyIndex = history.Count;
                
                string input = currentIndex + "\n";
                byte[] buffer = Encoding.UTF8.GetBytes(input);
                _stream.Write(buffer, 0, buffer.Length);
                _stream.Flush();

                Console.WriteLine();
                currentIndex = "";
            }
            else
            {
                Console.WriteLine();
            }
        }

        private void OnBackspace()
        {
            if (currentIndex.Length > 0)
            {
                currentIndex = currentIndex[..^1];
                RedrawLine();
            }
        }

        private void OnUpArrow()
        {
            if (history.Count > 0 && historyIndex > 0)
            {
                historyIndex--;
                currentIndex = history[historyIndex];
                RedrawLine();
            }
        }

        private void OnDownArrow()
        {
            if (historyIndex < history.Count -1)
            {
                historyIndex++;
                currentIndex = history[historyIndex];
                RedrawLine();
            }
        }

        private void OnEscape()
        {
            currentIndex = "";
            RedrawLine();
        }

    }
}







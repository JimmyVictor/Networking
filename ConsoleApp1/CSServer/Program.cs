using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSServer
{
    class Server
    {
        public static string data;

        public static void StartListening()
        {
            byte[] bytes = new Byte[1024];

            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = IPAddress.Loopback;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1234);

            Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                ServerSocket.Bind(iPEndPoint);
                ServerSocket.Listen(10);

                while(true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket ClientSocket = ServerSocket.Accept();

                    data = null;

                    int bytseRec = ClientSocket.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytseRec);

                    Console.WriteLine("Text received: {0}", data);

                    data += "   this is from the server!!!";

                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    ClientSocket.Send(msg);
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Press enter to continue...");
            Console.Read();
        }

        public static void StartReceiving()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = IPAddress.Loopback;
            IPEndPoint endPoint = new IPEndPoint(iPAddress, 1234);

            Socket s = new Socket(endPoint.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderRemote = (EndPoint)sender;

            s.Bind(endPoint);

            byte[] msg = new Byte[256];
            Console.WriteLine("Waiting to receive datagrams from client...");

            s.ReceiveFrom(msg, msg.Length, SocketFlags.None, ref senderRemote);
            string result = System.Text.Encoding.UTF8.GetString(msg);
            Console.WriteLine(result);
            s.Close();
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            //StartListening();
            StartReceiving();
        }
    } // end of class
} // end of name space

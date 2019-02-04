using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    class Client
    {
        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress iPAddress = IPAddress.Loopback;
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1234); // the server we want to connect to

                Socket senderSocket = new Socket(iPAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                try
                {
                    senderSocket.Connect(iPEndPoint);
                    Console.WriteLine("Socket connected to {0}", senderSocket.RemoteEndPoint.ToString());

                    string sMessage = Console.ReadLine();

                    byte[] msg = Encoding.ASCII.GetBytes(sMessage);

                    int bytesSent = senderSocket.Send(msg); // double check message before it is received

                    //int bytesRec = senderSocket.Receive(bytes); // populate the array with data it receives from the server
                    //Console.WriteLine("Enchoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    senderSocket.Shutdown(SocketShutdown.Both);
                    senderSocket.Close();
                }
                catch (ArgumentException ane)
                {
                    Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException: {0}", se.ToString());
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unexpected exception: {0}", e.ToString());
                }
            }// end of first try block
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception: {0}", e.ToString());
            }
            //StartClient();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                StartClient();
            }

            Console.WriteLine("Client shuting down.");
            Console.ReadLine();
        }
    }// end of class
}// end of namespace

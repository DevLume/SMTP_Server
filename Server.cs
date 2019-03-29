//TODO: Read about callbacks

//!ABOUT COMMAND LENGTHS -> CHECK IBM PAGE IN BOOKMARKS

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
namespace SMTP_Server
{
    public class Server
    {
        private TcpListener server;
        private int port;
        private IPAddress ipAddr;
        private int sessCount;
        public SessionManager sessMan;
        public volatile bool ServerOff;

        public Server()
        {
            sessMan = new SessionManager();
        }

        public void KeyboardListen()
        {
            while (!ServerOff)
            {
                string input = Console.ReadLine();
                if (string.Compare(input, "/exit") == 0 || string.Compare(input, "/EXIT") == 0)
                {
                    ServerOff = true;
                    KillServer();
                }
            }
        }

        public void KillServer()
        {
            for (int i = sessMan.SessionList.Count; i > 0; i--)
            {
                Session s = sessMan.SessionList.ElementAt(i - 1);
                s.Disconnect();
            }
            server.Stop();
            Environment.Exit(0);
        }

        public void StartListening(string ip, int port)
        {
            try
            {
                sessCount = 0;
                this.port = port;
                ipAddr = IPAddress.Parse(ip);
                server = new TcpListener(ipAddr, port);

                server.Start();

                Thread t = new Thread(KeyboardListen);
                t.Start();
              
                while (!ServerOff)
                {
                    Console.WriteLine("Active session count: {0}", sessMan.SessionList.Count);
                    Console.WriteLine("Waiting for connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    sessCount++;                 

                    Console.Write("Connected!");            
                    Session s = new Session(sessCount, client);
                    sessMan.InitSession(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured {0}", ex);
            }
            finally
            {
                server.Stop();
            }
        }    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SMTP_Server
{
    public class SessionManager
    {
        public List<Session> SessionList;

        public SessionManager()
        {
            SessionList = new List<Session>();    
        }

        public void InitSession(Session s)
        {
            SessionList.Add(s);
            s.DisconnectEvent += OnSessionDisconnect;

            s.sessionThread = new Thread(StartSession);

            s.sessionThread.Start(s);
        }

        public void StreamWrite(NetworkStream s, byte[] msg, int offset, int len)
        {
            string str = System.Text.Encoding.Default.GetString(msg);
            str += "\r\n";
            byte[] m = Encoding.ASCII.GetBytes(str);
            s.Write(m, offset, m.Length);
        }

        public void SendGreeting(NetworkStream stream)
        {
            SMTP_Message messg = new SMTP_Message((int)States.service_ready, "www.test.smtp.com", "Service ready");
            byte[] msg = Encoding.ASCII.GetBytes(messg.ToString());
            StreamWrite(stream, msg, 0, msg.Length);
        }

        public void StartSession(object session)
        {
            Console.WriteLine("New user has connected! ");
            Session s = (Session)session;
            TcpClient tempClient = s.client;
            NetworkStream stream = tempClient.GetStream();

            bool disconnect = false;
            int i = 0;
            string data = null;
            byte[] bytes = new byte[256];

            byte[] msg;
            SendGreeting(stream);

            StringBuilder tempSb = new StringBuilder();

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0 && !disconnect)
            {
                //Logic here
                data = Encoding.ASCII.GetString(bytes, 0, i);
                //data = data.ToUpper();

                if (string.Compare(data, "/exit") == 0 || string.Compare(data, "/EXIT") == 0)
                {
                    Console.WriteLine("Disconnect");
                    disconnect = true;

                    s.Disconnect();
                }
                else 
                {
                    ISMTP_Command command;
                    string error = null;
                    string response = null;
                    if (s.waitingForCommand)
                    {
                        command = s.ParseCommand(data, ref error, ref response);
                        if (command == null && error != null)
                        {
                            msg = Encoding.ASCII.GetBytes(error);
                            StreamWrite(stream, msg, 0, msg.Length);
                        }
                        else if (command != null)
                        {
                            SMTP_Message m = command.Launch(s); // Launch command changes state of a session                       
                            msg = Encoding.ASCII.GetBytes(m.ToString());
                            StreamWrite(stream, msg, 0, msg.Length);
                        }
                        else if (command == null)
                        {
                            if (!(i == 2 && string.Compare(data, "\r\n") == 0) &&
                                !(i == 21 &&
                                string.Compare(data, "??\u001f?? ??\u0018??'??\u0001??\u0003??\u0003") == 0))
                            {
                                SMTP_Message m = new SMTP_Message((int)States.command_not_implemented, "Bad command");
                                msg = Encoding.ASCII.GetBytes(m.ToString());
                                StreamWrite(stream, msg, 0, msg.Length);
                            }
                        }
                    }
                    else if (s.done)
                    {
                        s.Disconnect();
                    }
                    else
                    {
                        /*string lastSyms = data.Last(5);
                        if (string.Compare(lastSyms, "\r\n.\r\n") == 0)
                        {
                            tempSb.Append(data);
                            s.mailData = tempSb.ToString();
                            Console.Write(s.mailData);
                            s.waitingForCommand = true;
                            SMTP_Message m = new SMTP_Message((int)States.action_ok, "Message accepted for delivery.");
                            msg = Encoding.ASCII.GetBytes(m.ToString());
                            StreamWrite(stream, msg, 0, msg.Length);
                        }
                        else
                        {
                            tempSb.Append(data);
                        }*/
                        if (i == 1)
                        {
                            if (data[0] == '.')
                            {
                                s.mailData = tempSb.ToString();
                                Console.Write(s.mailData);
                                s.waitingForCommand = true;
                                SMTP_Message m = new SMTP_Message((int)States.action_ok, "Message accepted for delivery.");
                                msg = Encoding.ASCII.GetBytes(m.ToString());
                                StreamWrite(stream, msg, 0, msg.Length);
                            }
                        }
                        tempSb.Append(data);

                    }
                    
                }
            }
            tempClient.Close();
        }

        private void OnSessionDisconnect(object sender, DisconnectEventArgs e)
        {
            //find a session with needed id and abort it
            var x = from sess in SessionList
                    where sess.sessionID == e.SessID
                    select sess;

            try
            {
                Session s = x.First();
                s.sessionThread.Join(0);
                SessionList.Remove(s);
                Console.WriteLine("Session {0} disconnected successfully, there are {1} sessions left", s.sessionID, SessionList.Count);
            }
            catch (Exception)
            {
                Console.WriteLine("No alive sessions left.");
            }
        }
    }
}

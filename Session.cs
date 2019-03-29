using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SMTP_Server
{
    public class Session
    {
        public EventHandler<DisconnectEventArgs> DisconnectEvent;
        public int sessionID;
        public TcpClient client;
        public Thread sessionThread;

        public bool waitingForCommand = true;
        public bool done = false;

        public string clientDomain; 
        public string reversePath; 
        public List<string> forwardPath; // forwardPath address
        public string mailData; // mail text

        public Session(int id, TcpClient client)
        {
            sessionID = id;
            this.client = client;

            this.Reset();   
        }

        public void Reset()
        {
            clientDomain = string.Empty;
            reversePath = string.Empty;
            forwardPath = new List<string>();
            mailData = string.Empty;
        }

        public void Disconnect()
        {
            DisconnectEvent?.Invoke(this, new DisconnectEventArgs(sessionID, client, sessionThread));
        }

        //!NOTE:COMMANDS SHOULD BE PARSED NON-CASE-SENSITIVELY
        public ISMTP_Command ParseCommand(string cmdLine, ref string error, ref string response)
        {
            cmdLine.ToUpper();
            string[] cmds = cmdLine.Split(' ');
   
            //a ton of ifs incoming...
            //what if commands come in batch?

            if (string.Compare(cmds[0], "HELO") == 0)
            {
                if (cmds.Length == 2)
                {
                    return new SMTP_HELO(cmds[1]);
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "EHLO") == 0)
            {
                if (cmds.Length == 2)
                {
                    return new SMTP_EHLO(cmds[1]);
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "MAIL") == 0 && string.Compare(cmds[1], "FROM") == 0)
            {                
                if (cmds.Length == 3)
                {
                    return new SMTP_MAILFROM(cmds[2]);
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "RCPT") == 0 && string.Compare(cmds[1], "TO") == 0)
            {
                if (cmds.Length == 3)
                {
                    return new SMTP_RCPTO(cmds[2]);
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "DATA") == 0)
            {
                if (cmds.Length == 1)
                {
                    return new SMTP_DATA();
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "QUIT") == 0)
            {
                if (cmds.Length == 1)
                {
                    return new SMTP_QUIT();
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "NOOP") == 0)
            {
                if (cmds.Length == 1)
                {
                    return new SMTP_NOOP();
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "HELP") == 0)
            {
                if (cmds.Length == 1)
                {
                    return new SMTP_HELP();
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }
            else if (string.Compare(cmds[0], "RSET") == 0)
            {
                if (cmds.Length == 1)
                {
                    return new SMTP_RSET();
                }
                else
                {
                    return new SMTP_BAD_COMMAND();
                }
            }

            return null;
        }       
    }

    public class DisconnectEventArgs : EventArgs
    {
        public int SessID { get; set; }
        public TcpClient Client { get; set; }
        public Thread thread { get; set; }
        public DisconnectEventArgs(int i, TcpClient c, Thread t)
        {
            SessID = i;
            Client = c;
            thread = t;
        }
    }
}

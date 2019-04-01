
using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_Message
    {
        public int statusCode;
        public string sender;
        public string message;

        public List<string> Msg { get; set; }

        private string BuildMessage(int status, string message, string sender)
        {
            StringBuilder sb = new StringBuilder();

            if (sender != string.Empty)
            {
                sb.Append(status.ToString() + " " + sender + " " + message);
            }
            else
            {
                sb.Append(status.ToString() + "-" + message);
            }
            return sb.ToString();
        }

        public SMTP_Message(int status, string sender, string msg)
        {
            statusCode = status;
            this.sender = sender;
            message = msg;
            Msg = new List<string>();

            string temp = BuildMessage(status, msg, sender);
            Msg.Add(temp);
        }

        public SMTP_Message(int status, string msg)
        {
            statusCode = status;
            message = msg;
            sender = string.Empty;
            Msg = new List<string>();

            string temp = BuildMessage(status, msg, sender);
            Msg.Add(temp);
        }

        public SMTP_Message(List<SMTP_Message> msg)
        {
            Msg = new List<string>();
            foreach (SMTP_Message m in msg)
            {
                Msg.Add(m.StringVersion());
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string t in Msg)
            {
                sb.Append(t + "\r\n");
            }
            sb[sb.Length - 1] = ' ';
            sb[sb.Length - 2] = ' ';
            return sb.ToString();
        }
    }

    public static class MessageExtension
    {
        public static string StringVersion(this SMTP_Message source)
        {
            if (source.sender != string.Empty)
            {
                return source.statusCode + " " + source.sender + " " + source.message;
            }
            else
            {
                return source.statusCode + "-" + source.message;
            }
        }
    }
}

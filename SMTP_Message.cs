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
        
        public SMTP_Message(int status, string sender, string msg)
        {
            statusCode = status;
            this.sender = sender;
            message = msg;
        }

        public SMTP_Message(int status, string msg)
        {
            statusCode = status;
            message = msg;
            sender = string.Empty;
        }

        public override string ToString()
        { 
            return statusCode.ToString() + " " + sender + " " + message;
        }
    }
}

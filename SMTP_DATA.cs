using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_DATA : ISMTP_Command
    {
        public SMTP_DATA()
        {

        }

        public SMTP_Message Launch(Session s)
        {
            s.waitingForCommand = false;
            return new SMTP_Message((int)States.start_mail, "Send message content, end with <CRLF>.<CRLF>");
        }
    }
}

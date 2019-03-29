using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_QUIT : ISMTP_Command
    {
        public SMTP_QUIT()
        {
        }

        public SMTP_Message Launch(Session s)
        {
            s.done = true;
            s.waitingForCommand = false;
            s.Disconnect();
            return new SMTP_Message((int)States.service_closing, "Bye");
        }
    }
}

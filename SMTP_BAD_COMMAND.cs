using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_BAD_COMMAND : ISMTP_Command
    {
        public SMTP_Message Launch(Session s)
        {
            return new SMTP_Message((int)States.bad_command, "BAD COMMAND");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_NOOP : ISMTP_Command
    {
        public SMTP_Message Launch(Session s)
        {
            return new SMTP_Message((int)States.action_ok, "OK");
        }
    }
}

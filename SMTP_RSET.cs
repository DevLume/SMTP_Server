using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_RSET : ISMTP_Command
    {
        public SMTP_Message Launch(Session s)
        {
            s.Reset();
            return new SMTP_Message((int)States.action_ok, "OK");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_HELO : ISMTP_Command
    {
        public string domainName;

        public SMTP_HELO(string domainName)
        {
           this.domainName = domainName;
        }

        public SMTP_Message Launch(Session s)
        {
            s.Reset();
            s.clientDomain = domainName;
            return new SMTP_Message((int)States.action_ok, domainName, "Hello, dear friend!");
        }
    }
}

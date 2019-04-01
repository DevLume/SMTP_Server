using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_AUTH : ISMTP_Command
    {
        public SMTP_AUTH(IAUTH_TYPE authType)
        { }

        public SMTP_Message Launch(Session s)
        {
            throw new NotImplementedException();
        }
    }
}

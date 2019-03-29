using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public interface ISMTP_Command
    {
        SMTP_Message Launch(Session s);
    }
}

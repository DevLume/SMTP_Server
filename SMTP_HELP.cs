using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_HELP : ISMTP_Command
    {
        private string GetHelpText()
        {
            return "Commands that are implemented: HELO, MAIL FROM, RCPT TO, DATA, QUIT, NOOP, HELP";
        }

        public SMTP_Message Launch(Session s)
        {
            return new SMTP_Message((int)States.action_ok, GetHelpText());
        }
    }
}

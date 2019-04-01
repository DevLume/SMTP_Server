using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_EHLO : ISMTP_Command
    {
        public string domainName;

        public SMTP_EHLO(string domainName)
        {
            this.domainName = domainName;
        }

        public SMTP_Message Launch(Session s)
        {
            s.Reset();
            s.clientDomain = domainName;

            List<SMTP_Message> msg = new List<SMTP_Message>();
            msg.Add(new SMTP_Message((int)States.action_ok, "Hello, Dear friend!"));
            msg.InsertRange(msg.Count, EnableExtensions(s));

            return new SMTP_Message(msg);
        }

        private List<SMTP_Message> EnableExtensions(Session s)
        {
            List<SMTP_Message> temp = new List<SMTP_Message>();

            s.extendedMode = true;

            temp.Add(new SMTP_Message((int)States.action_ok, "STARTTLS"));

            return temp;
        }
    }
}

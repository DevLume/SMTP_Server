using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SMTP_Server
{
    public class MailTypeResolver
    {
        public IMail GetMailType(string receiver)
        {
            string domain = receiver.Split('@')[1];
            string hostname = Dns.GetHostName();

            if (string.Compare(domain, hostname) == 0)
            {
                return new FileMail();
            }
            else
            {
                return new Mailer(domain);
            }
        }
    }
}

using DnsClient;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTP_Server
{
    public class Mailer : IMail
    {
        private string Domain { get; set; }

        public Mailer(string domain)
        {
            Domain = domain;
        }

        public void Send(string from, string to, string data)
        {          
            throw new NotImplementedException();
        }


        private async Task<string> ResolveMX(string domain)
        {
            var lookup = new LookupClient();
            var result = await lookup.QueryAsync(domain, QueryType.ANY);
            var record = result.Answers.MxRecords().FirstOrDefault();
            var address = record?.Exchange;

            return address.ToString();
        }     
    }
}

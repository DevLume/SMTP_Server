using DnsClient;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP_Server
{
    public class Program
    {            
        public static void Main(string[] args)
        {
            Server con = new Server();

            FileMail.QueueNr = 0;

            IPAddress ipAddr = Dns.GetHostAddresses(System.Environment.UserDomainName)[2];

            Console.WriteLine("Listening at {0} {1} {2}", ipAddr, 9025, System.Environment.UserDomainName);
            con.StartListening(ipAddr.ToString(), 9025);

            Console.Read();
        }        
    }

    public static class StringExtension
    {
        public static string Last(this string source, int tail_len)
        {
            if (tail_len >= source.Length)
            {
                return source;
            }
            return source.Substring(source.Length - tail_len);
        }

        public static bool IsEmail(this string source)
        {
            Regex rx = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = rx.Match(source);

            return match.Success;
        }

        public static bool IsLocalDomain(this string source)
        {
            string hostname = System.Environment.UserDomainName;
            string[] temp = source.Split('@');
            return (string.Compare(hostname, temp[1]) == 0);
        }

        public static string BuildOneString(this List<string> source)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in source)
            {
                sb.Append(s);
                sb.Append(";");
            }

            return sb.ToString();
        }
    }
}

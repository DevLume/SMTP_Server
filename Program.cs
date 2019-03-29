using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

//?TODO:
//?Implement multiple recipients

namespace SMTP_Server
{
    public class Program
    {
        //TODO: Create a program flow graph        
        public static void Main(string[] args)
        {
            //Single client server
            Server con = new Server();

            con.StartListening("127.0.0.1", 25);         
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
    }
}

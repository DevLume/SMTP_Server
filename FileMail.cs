using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SMTP_Server
{
    public class FileMail : IMail
    {
        public static int QueueNr { get; set; }

        public FileMail()
        {
        }

        public void Send(string from, string to, string data)
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

            StringBuilder sb = new StringBuilder();

            sb.Append(path);
            sb.Append(@"\mail\");

            if (!Directory.Exists(sb.ToString()))
            {
                System.IO.Directory.CreateDirectory(sb.ToString());
            }

            sb.Append(QueueNr.ToString() + ".txt"); 
            QueueNr++;

            if (File.Exists(sb.ToString()))
            {
                try
                {
                    File.Delete(sb.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured, could not delete file:{0}", ex);
                }
            }

            try
            {
                using (FileStream fs = File.Create(sb.ToString()))
                {
                    byte[] databytes = Encoding.ASCII.GetBytes(data);
                    fs.Write(databytes, 0, databytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured, could not create file:{0}", ex);
            }
        }
    }
}

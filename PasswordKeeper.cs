using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SMTP_Server
{
    public class PasswordKeeper
    {
        public PasswordKeeper()
        {
        }

        public string GetRelayCredentials()
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            StringBuilder sb = new StringBuilder();
            sb.Append(path);
            sb.Append(@"\creds\");

            if (!Directory.Exists(sb.ToString()))
            {
                Console.WriteLine("No credentials saved");
                return null;
            }
            sb.Append(@"crd.txt");

            if (File.Exists(sb.ToString()))
            {
                try
                {
                    byte[] byteRead = File.ReadAllBytes(sb.ToString());
                    string data = System.Text.Encoding.ASCII.GetString(byteRead);
                    string[] temp = data.Split(' ');
                    Cryptor cryptor = new Cryptor();
                    string cred = cryptor.DehashString(temp[0], temp[1]);
                    return cred;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Coul not retrieve credentials: {0}", ex);
                }
            }
            return null;
        }

        public void SetRelayCredentials(string creds, string key)
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

            StringBuilder sb = new StringBuilder();

            sb.Append(path);
            sb.Append(@"\creds\");

            if (!Directory.Exists(sb.ToString()))
            {
                System.IO.Directory.CreateDirectory(sb.ToString());
            }

            sb.Append(@"crd.txt");

            if (File.Exists(sb.ToString()))
            {
                try
                {
                    File.Delete(sb.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured, could not delete creds file: {0}", ex);
                }
            }

            try
            {
                using (FileStream fs = File.Create(sb.ToString()))
                {
                    Cryptor cryptor = new Cryptor();
                    string hash = cryptor.HashString(creds, key);
                    byte[] databytes = Encoding.ASCII.GetBytes(hash);
                    fs.Write(databytes, 0, databytes.Length);
                    databytes = Encoding.ASCII.GetBytes(" " + key);
                    fs.Write(databytes, 0, databytes.Length);
                }
            } catch (Exception ex)     
            {
                //¯\_(ツ)_/¯
                Console.WriteLine("An error while inserting creds into a file occured: {0}", ex);
            }
        }
    }
}

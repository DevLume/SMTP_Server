using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SMTP_Server
{
    public class SMTP_MAILFROM : ISMTP_Command
    {
        public string reversePath;
        private bool _Error { get; set; }

        public SMTP_MAILFROM(string sender)
        {
            if (sender[0] == '<' && sender.Last(1)[0] == '>')
            {
                _Error = false;
                string temp = sender.Substring(1, sender.Length - 2);
                reversePath = temp;
            }
            else
            {
                _Error = true;
            }
        }

        public SMTP_Message Launch(Session s)
        {
            if (s.clientDomain != string.Empty && !_Error)
            {            
                if (reversePath.IsEmail())
                {
                    s.mailData = string.Empty;
                    s.forwardPath = new List<string>();
                    s.reversePath = reversePath;
                    return new SMTP_Message((int)States.action_ok, "OK");
                }
                else
                {
                    return new SMTP_Message((int)States.params_error, "BAD PARAMETERS: wrong format");
                }               
            }
            else if (_Error)
            {
                return new SMTP_Message((int)States.params_error, "BAD PARAMETERS");
            }
            else
            {
                return new SMTP_Message((int)States.bad_sequence, "BAD COMMAND SEQUENCE");
            }
        }
    }
}

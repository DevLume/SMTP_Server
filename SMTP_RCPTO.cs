using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public class SMTP_RCPTO : ISMTP_Command
    {
        public string receiver;

        private bool _Error { get; set; }
        public SMTP_RCPTO(string receiver)
        {
            if (receiver[0] == '<' && receiver.Last(1)[0] == '>')
            {
                _Error = false;
                string temp = receiver.Substring(1, receiver.Length - 2);
                this.receiver = temp;
            }
            else
            {
                _Error = true;
            }
        }

        public SMTP_Message Launch(Session s)
        {
            if (s.clientDomain != string.Empty && !_Error && s.reversePath != string.Empty)
            {
                if (receiver.IsEmail())
                {
                    s.forwardPath.Add(receiver);
                }
                else if (receiver.IsLocalDomain())
                {
                    s.forwardPath.Add(receiver);
                }
                else
                {
                    return new SMTP_Message((int)States.params_error, "BAD PARAMETERS: wrong format");
                }
                return new SMTP_Message((int)States.action_ok, "OK");
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

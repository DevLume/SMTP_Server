using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public interface IMail
    {
        void Send(string from, string to, string data);
    }
}

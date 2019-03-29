using System;
using System.Collections.Generic;
using System.Text;

namespace SMTP_Server
{
    public enum States
    {
        nonstandard_success = 200,
        sys_status = 211,
        help = 214,
        service_ready = 220,
        service_closing = 221,
        action_ok = 250,
        will_forward = 251,
        cannot_verify_but_ok = 252,
        start_mail = 354,
        service_not_available = 421,
        mailbox_unavailable_no_mail_action = 450,
        local_error = 451,
        insufficient_storage = 452,
        bad_command = 500,
        params_error = 501,
        command_not_implemented = 502,
        bad_sequence = 503,
        param_not_implemented = 504,
        no_mail_accept = 521,
        access_denied = 530,
        mailbox_unavailable_no_action = 550,
        usr_not_local = 551, 
        exceeded_storage_allocation = 552,
        mailbox_name_bad = 553,
        transaction_failed = 554
    }
}

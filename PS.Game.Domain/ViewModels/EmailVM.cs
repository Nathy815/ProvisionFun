using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class EmailVM
    {
        public readonly string EmailFrom = "no_reply@provisionfun.com.br";
        public readonly string SMTP = "smtp.provisionfun.com.br";
        public readonly int Port = 587;
        public readonly bool IsBodyHTML = true;
        public readonly bool UseSSL = false;
        public readonly string User = "no_reply@provisionfun.com.br";
        public readonly string Password = "jTip8wNU";
        public List<string> EmailTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public EmailVM(string emailTo, 
                       string subject, 
                       string message)
        {
            EmailTo = new List<string>() { emailTo };
            Subject = subject;
            Message = message;
        }
    }
}

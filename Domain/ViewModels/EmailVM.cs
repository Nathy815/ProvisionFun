using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class EmailVM
    {
        public readonly string EmailFrom = "nathy_fox-815@hotmail.com";
        public readonly string SMTP = "smtp.office365.com";
        public readonly int Port = 587;
        public readonly bool IsBodyHTML = true;
        public readonly bool UseSSL = true;
        public readonly string User = "nathy_fox-815@hotmail.com";
        public readonly string Password = "5456Next!@#";
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

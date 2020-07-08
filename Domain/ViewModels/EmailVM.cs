using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class EmailVM
    {
        public readonly string EmailFrom = "teste@provisionservice.com.br";
        public readonly string SMTP = "mail.provisionservice.com.br";
        public readonly int Port = 587;
        public readonly bool IsBodyHTML = true;
        public readonly bool UseSSL = true;
        public readonly string User = "teste@provisionservice.com.br";
        public readonly string Password = "Teste123@";
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

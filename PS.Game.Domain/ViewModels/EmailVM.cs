using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class EmailVM
    {
        public string EmailFrom { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }
        public readonly bool IsBodyHTML = true;
        public bool UseSSL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public List<string> EmailTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public EmailVM(string emailTo, 
                       string subject, 
                       string message,
                       IConfiguration _configuration)
        {
            EmailFrom = _configuration.GetSection("EmailSettings:EmailFrom").Value;
            SMTP = _configuration.GetSection("EmailSettings:SMTP").Value;
            Port = Convert.ToInt32(_configuration.GetSection("EmailSettings:Port").Value);
            UseSSL = Convert.ToBoolean(_configuration.GetSection("EmailSettings:UseSSL").Value);
            User = _configuration.GetSection("EmailSettings:User").Value;
            Password = _configuration.GetSection("EmailSettings:Password").Value;
            EmailTo = new List<string>() { emailTo };
            Subject = subject;
            Message = message;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class TemplatePlayerVM
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public IFormFile Document { get; set; }
    }
}

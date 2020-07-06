using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class AddressVM
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Domain.ViewModels
{
    public class ShippingVM
    {
        public string data { get; set; }

        public ShippingVM(string base64)
        {
            data = base64;
        }
    }
}

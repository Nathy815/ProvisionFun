using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Domain.ViewModels
{
    public class LoginVM
    {
        public string Token { get; set; }

        public LoginVM(string token)
        {
            Token = token;
        }
    }
}

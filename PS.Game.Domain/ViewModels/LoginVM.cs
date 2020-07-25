using System;
using System.Collections.Generic;
using System.Text;

namespace PS.Game.Domain.ViewModels
{
    public class LoginVM
    {
        public Guid Id { get; set; }
        public string Token { get; set; }

        public LoginVM(Guid id, string token)
        {
            Id = id;
            Token = token;
        }
    }
}

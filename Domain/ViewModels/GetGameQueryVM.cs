using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetGameQueryVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Plataform { get; set; }

        public GetGameQueryVM(Game game)
        {
            Id = game.Id;
            Name = game.Name;
            Plataform = game.Plataform;
        }
    }
}

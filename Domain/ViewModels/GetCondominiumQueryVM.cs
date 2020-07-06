using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class GetCondominiumQueryVM
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public GetCondominiumQueryVM() { }

        public GetCondominiumQueryVM(Condominium condominium)
        {
            Id = condominium.Id;
            Name = condominium.Name;
            ZipCode = condominium.ZipCode;
            Address = condominium.Address;
            Number = condominium.Number;
            City = condominium.City;
            State = condominium.State;
        }

        public GetCondominiumQueryVM(AddressVM address)
        {
            ZipCode = address.cep.Trim();
            Address = address.logradouro.Trim();
            City = address.localidade.Trim();
            State = address.uf.Trim();
        }
    }
}

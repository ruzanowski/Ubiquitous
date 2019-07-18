using System;
using System.Runtime.Serialization;
using MediatR;

namespace U.ProductService.Application.Commands
{
    public class CreateProductCommand : IRequest<bool>
    {
        [DataMember]
        public DateTime? DueDate { get; private set; }
        
        [DataMember]
        public string City { get; private set; }

        [DataMember]
        public string Street { get; private set; }

        [DataMember]
        public string State { get; private set; }

        [DataMember]
        public string Country { get; private set; }

        [DataMember]
        public string ZipCode { get; private set; }

        public CreateProductCommand(DateTime? dueDate, string city, string street, string state, string country, string zipcode)
        {
            DueDate = dueDate;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }
    }
}

using System.Runtime.Serialization;
using MediatR;

namespace U.ProductService.Application.Commands
{
    public class CreateProductCommand : IRequest<bool>
    {
        [DataMember]
        public string City { get; private set; }

        [DataMember]
        public string Street { get; private set; }

        [DataMember]
        public string Country { get; private set; }

        [DataMember]
        public string ZipCode { get; private set; }

        public CreateProductCommand(string city, string street, string country, string zipcode)
        {
            City = city;
            Street = street;
            Country = country;
            ZipCode = zipcode;
        }
    }
}

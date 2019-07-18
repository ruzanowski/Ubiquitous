using System;
using U.ProductService.Domain.Events;
using U.ProductService.Domain.SeedWork;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace U.ProductService.Domain.Aggregates.Product
{
    public class Product : Entity, IAggregateRoot
    {
        public DateTime ProductDueDate { get; private set; }

        public Address Address { get; private set; }
        public int? BuyerId { get; private set; }
        public Guid ManufacturerId { get; private set; }

        public bool IsDraft { get; private set; }

        public static Product NewDraft()
        {
            var order = new Product();
            order.IsDraft = true;
            return order;
        }

        protected Product()
        {
            IsDraft = false;
            Id = Guid.NewGuid();
        }

        public Product(Guid manufacturerId, Address address, DateTime? productDueDate, int? buyerId = null) : this()
        {
            ManufacturerId = manufacturerId; 
            Address = address;
            ProductDueDate = productDueDate ?? DateTime.Now;
            
            var productAddedEvent = new ProductAddedDomainEvent(Id, manufacturerId);        
            
            AddDomainEvent(productAddedEvent);
        }

        public void SetBuyerId(int id)
        {
            BuyerId = id;
        }
    }
}


using U.EventBus.Events;

namespace U.ProductService.Application.IntegrationEvents.EventHandling
{
    public class NewProductFetchedIntegrationEvent : IntegrationEvent
    {
        public string Name { get; private set; }
        public int ManufacturerId { get; private set; }
        public string ProductUniqueCode { get; private set; }
        public int InStock { get; private set; }
        public decimal PriceInTax { get; private set; }
        public string Description { get; private set; }
        public decimal Length { get; private set; }
        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Weight { get; private set; }
        public int? MainPictureId { get; private set; }
        public int CategoryId { get; private set; }
        public new string Id { get; private set; } // customerId.productId

        public NewProductFetchedIntegrationEvent(string name, int manufacturerId, string productUniqueCode, int inStock, decimal priceInTax, string description, decimal length, decimal width, decimal height, decimal weight, int? mainPictureId, int categoryId, string id)
        {
            Name = name;
            ManufacturerId = manufacturerId;
            ProductUniqueCode = productUniqueCode;
            InStock = inStock;
            PriceInTax = priceInTax;
            Description = description;
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            MainPictureId = mainPictureId;
            CategoryId = categoryId;
            Id = id;
        }
        
        public string GetUniqueId => $"{Id}.{ProductUniqueCode}";
    }
}
using U.Common.Subscription;

namespace U.EventBus.Events.Fetch
{
    public class NewProductFetchedIntegrationEvent : IntegrationEvent
    {
        public new string Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public string BarCode { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int CategoryId { get; set; }
        public override string MethodTag => nameof(NewProductFetchedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.NewProductFetched;
        public string ExternalSourceName { get; set; }

        public NewProductFetchedIntegrationEvent()
        {
        }

        public NewProductFetchedIntegrationEvent(string name,
            int manufacturerId,
            string barCode,
            int inStock,
            decimal priceInTax,
            string description,
            decimal length,
            decimal width,
            decimal height,
            decimal weight,
            int categoryId,
            string externalId,
            string externalSourceName)
        {
            Name = name;
            ManufacturerId = manufacturerId;
            BarCode = barCode;
            StockQuantity = inStock;
            Price = priceInTax;
            Description = description;
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            CategoryId = categoryId;
            Id = externalId;
            ExternalSourceName = externalSourceName;
        }
    }
}
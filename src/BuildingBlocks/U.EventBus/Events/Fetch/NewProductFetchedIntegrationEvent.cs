using U.Common.Subscription;

namespace U.EventBus.Events.Fetch
{
    public class NewProductFetchedIntegrationEvent : IntegrationEvent
    {
        public new string Id { get; set; }
        public string Name { get; private set; }
        public int ManufacturerId { get; private set; }
        public string BarCode { get; private set; }
        public int StockQuantity { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public bool IsAvailable { get; private set; }
        public decimal Length { get; private set; }
        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Weight { get; private set; }
        public int CategoryId { get; private set; }
        public override string MethodTag => nameof(NewProductFetchedIntegrationEvent);
        public override IntegrationEventType EventType => IntegrationEventType.NewProductFetched;
        public string ExternalSourceName { get; private set; }

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
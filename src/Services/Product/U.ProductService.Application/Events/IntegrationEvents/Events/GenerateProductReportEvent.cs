using System;
using System.Collections.Generic;
using U.EventBus.Events;

namespace U.ProductService.Application.Events.IntegrationEvents.Events
{
    public class GenerateProductReportEvent : IntegrationEvent
    {
        public IList<ReportProductPayload> Products { get; set; }
    }

    public class ReportProductPayload
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
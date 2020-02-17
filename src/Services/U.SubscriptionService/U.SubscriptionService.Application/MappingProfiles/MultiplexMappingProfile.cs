using AutoMapper;
using U.EventBus.Events.Product;

namespace U.SubscriptionService.Application.MappingProfiles
{
    public class MultiplexMappingProfile : Profile
    {
        public MultiplexMappingProfile()
        {
            CreateMap<ProductAddedIntegrationEvent, ProductAddedSignalRIntegrationEvent>();
            CreateMap<ProductAddedIntegrationEvent, ProductAddedEmailIntegrationEvent>();

            CreateMap<ProductPublishedIntegrationEvent, ProductPublishedSignalRIntegrationEvent>();
            CreateMap<ProductPublishedIntegrationEvent, ProductPublishedEmailIntegrationEvent>();

            CreateMap<ProductPropertiesChangedIntegrationEvent, ProductPropertiesChangedSignalRIntegrationEvent>();
            CreateMap<ProductPropertiesChangedIntegrationEvent, ProductPropertiesChangedEmailIntegrationEvent>();

        }
    }
}
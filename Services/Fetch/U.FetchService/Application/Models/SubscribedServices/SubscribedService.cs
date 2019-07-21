namespace U.FetchService.Application.Models.SubscribedServices
{
    public class SubscribedService : ISubscribedService
    {
        public IService Service { get; set; }
    }
}
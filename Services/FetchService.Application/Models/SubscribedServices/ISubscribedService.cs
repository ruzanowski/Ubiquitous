namespace U.FetchService.Application.Models.SubscribedServices
{
    public interface ISubscribedService
    {
        IService Service { get; set; }
    }
}
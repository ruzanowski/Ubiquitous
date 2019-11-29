using System;
using System.Threading.Tasks;

namespace U.NotificationService.Application.Services.Subscription
{
    public interface ISubscriptionService
    {
        Task BindConnectionToUserAsync(Guid userId, string connectionId);
        Task UnbindConnectionToUserAsync(Guid userId, string connectionId);
    }
}
using System;
using System.Threading.Tasks;
using RestEase;
using U.Common.Subscription;

namespace U.NotificationService.Application.SignalR.Services.Clients
{
    public interface ISubscriptionService
    {
        [Post("api/subscription/signalr/{userId}/bind")]
        [AllowAnyStatusCode]
        Task BindConnectionToUserAsync(Guid userId, string connectionId);

        [Post("api/subscription/signalr/{userId}/unbind")]
        [AllowAnyStatusCode]
        Task UnbindConnectionToUserAsync(Guid userId,  string connectionId);

        [Get("api/subscription/preferences/{userId}")]
        [AllowAnyStatusCode]
        Task<Preferences> GetMyPreferencesAsync(Guid userId);
    }
}
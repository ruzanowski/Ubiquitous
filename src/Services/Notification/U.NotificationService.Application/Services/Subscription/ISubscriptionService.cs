using System;
using System.Threading.Tasks;
using RestEase;
using U.Common.Jwt;
using U.Common.Subscription;

namespace U.NotificationService.Application.Services.Subscription
{
    public interface ISubscriptionService
    {
        [Post("api/subscription/signalrconnection/bind")]
        [AllowAnyStatusCode]
        Task BindConnectionToUserAsync(Guid userId, string connectionId);
        [Post("api/subscription/signalrconnection/unbind")]
        [AllowAnyStatusCode]
        Task UnbindConnectionToUserAsync(Guid userId, string connectionId);

        [Get("api/subscription/preferences/me")]
        [AllowAnyStatusCode]
        Task<Preferences> GetMyPreferencesAsync();
    }
}
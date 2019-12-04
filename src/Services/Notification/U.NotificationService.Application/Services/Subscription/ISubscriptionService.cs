using System;
using System.Threading.Tasks;
using RestEase;
using U.Common.Jwt;
using U.Common.Subscription;

namespace U.NotificationService.Application.Services.Subscription
{
    public interface ISubscriptionService
    {
        [Post("api/subscription/signalr/me/bind")]
        [AllowAnyStatusCode]
        Task BindConnectionToUserAsync(string connectionId);

        [Post("api/subscription/signalr/me/unbind")]
        [AllowAnyStatusCode]
        Task UnbindConnectionToUserAsync(string connectionId);

        [Get("api/subscription/preferences/me")]
        [AllowAnyStatusCode]
        Task<Preferences> GetMyPreferencesAsync();
    }
}
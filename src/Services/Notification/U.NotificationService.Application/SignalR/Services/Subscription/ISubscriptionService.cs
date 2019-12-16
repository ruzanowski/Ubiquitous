using System;
using System.Threading.Tasks;
using RestEase;
using U.Common.Subscription;

namespace U.NotificationService.Application.SignalR.Services.Subscription
{
    public interface ISubscriptionService
    {
        [Post("api/subscription/signalr/internal/bind")]
        [AllowAnyStatusCode]
        Task BindConnectionToUserAsync(Guid userId, string connectionId);

        [Post("api/subscription/signalr/internal/unbind")]
        [AllowAnyStatusCode]
        Task UnbindConnectionToUserAsync(Guid userId,  string connectionId);

        [Get("api/subscription/preferences/me/signalr")]
        [AllowAnyStatusCode]
        Task<Preferences> GetMyPreferencesAsync(Guid userId);
    }
}
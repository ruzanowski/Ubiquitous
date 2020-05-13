using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.Common.Subscription;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.SignalR.Services.WelcomeNotifications
{
    public interface IWelcomeNotificationsService
    {
        Task<List<Notification>> LoadWelcomeMessages(Preferences preferences, Guid userId);
    }
}
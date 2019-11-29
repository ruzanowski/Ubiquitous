using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.Services.WelcomeNotifications
{
    public interface IWelcomeNotificationsService
    {
        Task<List<Notification>> LoadWelcomeMessages(Guid userId);
    }
}
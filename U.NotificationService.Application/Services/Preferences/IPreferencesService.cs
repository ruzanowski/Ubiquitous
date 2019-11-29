using System.Threading.Tasks;
using U.NotificationService.Domain.Entities;

namespace U.NotificationService.Application.Services.Preferences
{
    public interface IPreferencesService
    {
        Task<int> NumberOfWelcomeMessages();
        Task<bool> DoNotNotifyAnyoneAboutMyActivity();
        Task<bool> OrderByCreationTimeDescending();
        Task<bool> OrderByImportancyDescending();
        Task<bool> SeeReadNotifications();
        Task<bool> SeeUnreadNotifications();
        Task<Importancy> MinimalImportancyLevel();
    }

    public class PreferencesService : IPreferencesService
    {
        public async Task<int> NumberOfWelcomeMessages()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DoNotNotifyAnyoneAboutMyActivity()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> OrderByCreationTimeDescending()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> OrderByImportancyDescending()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SeeReadNotifications()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SeeUnreadNotifications()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Importancy> MinimalImportancyLevel()
        {
            throw new System.NotImplementedException();
        }
    }
}
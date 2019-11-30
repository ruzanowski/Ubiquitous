using U.NotificationService.Domain.Entities;

namespace U.SubscriptionService.Domain
{
    public class Preferences
    {
        public int NumberOfWelcomeMessages { get; set; }
        public bool DoNotNotifyAnyoneAboutMyActivity { get; set; }
        public bool OrderByCreationTimeDescending { get; set; }
        public bool OrderByImportancyDescending { get; set; }
        public bool SeeReadNotifications { get; set; }
        public bool SeeUnreadNotifications { get; set; }
        public Importancy MinimalImportancyLevel { get; set; }
    }
}
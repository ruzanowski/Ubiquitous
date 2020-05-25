namespace U.Common.Subscription
{
    public class Preferences
    {
        public Preferences()
        {
            NumberOfWelcomeMessages = 50;
            DoNotNotifyAnyoneAboutMyActivity = false;
            OrderByCreationTimeDescending = true;
            OrderByImportancyDescending = true;
            SeeReadNotifications = true;
            SeeUnreadNotifications = true;
            MinimalImportancyLevel = Importancy.Trivial;
        }

        public int NumberOfWelcomeMessages { get; set; }
        public bool DoNotNotifyAnyoneAboutMyActivity { get; set; }
        public bool OrderByCreationTimeDescending { get; set; }
        public bool OrderByImportancyDescending { get; set; }
        public bool SeeReadNotifications { get; set; }
        public bool SeeUnreadNotifications { get; set; }
        public Importancy MinimalImportancyLevel { get; set; }
    }
}
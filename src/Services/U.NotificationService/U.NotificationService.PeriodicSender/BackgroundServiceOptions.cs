namespace U.NotificationService.PeriodicSender
{
    public class BackgroundServiceOptions
    {
        public bool Enabled { get; set; } = true;
        public int RefreshSeconds { get; set; }
    }
}
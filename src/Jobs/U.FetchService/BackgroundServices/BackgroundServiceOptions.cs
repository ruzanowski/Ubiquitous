namespace U.FetchService.BackgroundServices
{
    public class BackgroundServiceOptions
    {
        public bool Enabled { get; set; } = false;
        public int RefreshSeconds { get; set; }
    }
}
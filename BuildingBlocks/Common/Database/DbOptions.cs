namespace U.Common.Database
{
    public class DbOptions
    {
        public string Connection { get; set; }
        public DbType Type { get; set; }
        public bool AutoMigration { get; set; } = false;
        public bool Seed { get; set; } = false;
    }
}
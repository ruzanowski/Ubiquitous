namespace U.Common.NetCore.Database
{
    public class DbOptions
    {
        public string Connection { get; set; }
        public DbType Type { get; set; }
        public bool AutoMigration { get; set; } = true;
        public bool Seed { get; set; } = true;
    }
}
namespace U.Common.NetCore.EF
{
    public class DbOptions
    {
        public string Connection { get; set; }
        public DbType Type { get; set; }
        public bool AutoMigration { get; set; } = true;
        public bool Seed { get; set; } = true;
        public bool InTests { get; set; } = false;
        public bool IntegrationTests { get; set; } = false;
        public string TestConnection { get; set; } = null;
    }
}
namespace U.Common.NetCore.Cache
{
    public class RedisOptions
    {
        public string ServiceName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int ConnectRetry { get; set; }
        public bool AllowAdmin { get; set; }
        public bool ResolveDns { get; set; }
        public bool AbortOnConnectFail { get; set; }
        public bool UseInMemoryAsPrimaryCache { get; set; } = true;
        public bool Enabled { get; set; } = true;
    }
}
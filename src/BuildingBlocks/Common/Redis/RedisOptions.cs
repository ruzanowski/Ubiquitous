namespace U.Common.Redis
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
    }
}
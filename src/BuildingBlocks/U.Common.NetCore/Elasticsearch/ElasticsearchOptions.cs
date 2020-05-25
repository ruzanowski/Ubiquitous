namespace U.Common.NetCore.Elasticsearch
{
    public class ElasticsearchOptions
    {
        public bool Enabled { get; set; }
        // ReSharper disable once CA1056
        public string Uri { get; set; }
        public bool BasicAuthEnabled { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
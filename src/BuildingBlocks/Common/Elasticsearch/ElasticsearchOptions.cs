namespace U.Common.Elasticsearch
{
    public class ElasticsearchOptions
    {
        public bool Enabled { get; set; }
        public string Uri { get; set; }
        public bool BasicAuthEnabled { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
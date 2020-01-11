using System.Collections.Generic;

namespace U.Common.Observability.Influx
{
    public class InfluxOptions
    {
        public bool Enabled { get; set; }

        public string Uri { get; set; }
        public string Database { get; set; }
        public int Interval { get; set; }
        public IDictionary<string, string> Tags { get; set; }
    }
}
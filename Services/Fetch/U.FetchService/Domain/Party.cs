using Microsoft.EntityFrameworkCore;

// ReSharper disable All

namespace U.FetchService.Domain
{
    [Owned]
    public class Party
    {
        private Party()
        {
            
        }
        private Party(string name, string ip, int port, string protocol)
        {
            Name = name;
            Ip = ip;
            Port = port;
            Protocol = protocol;
        }
        
        public string Name { get; protected set; }
        public string Ip { get; protected set; }
        public int Port { get; protected set; }
        public string Protocol { get; protected set; }
        
        public static class Factory
        {
            public static Party Create(string name, string ip, int port, string protocol)
            {
                return new Party(name, ip, port, protocol);
            }
        }
    }
}
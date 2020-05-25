using U.Common.NetCore.Mvc;

namespace U.Common.NetCore.Consul
{
    public interface IConsulServiceDifferentator
    {
        bool IsTheSame(string consulRegisteredName);
    }

    public class ConsulServiceDiffentator : IConsulServiceDifferentator
    {
        private readonly ISelfInfoService _idService;
        private readonly ConsulOptions _consulOptions;

        public ConsulServiceDiffentator(ISelfInfoService idService, ConsulOptions consulOptions)
        {
            _idService = idService;
            _consulOptions = consulOptions;
        }

        public bool IsTheSame(string consulRegisteredName)
        {
            var serviceId = _idService.Id;
            var name = _consulOptions.Service;
            var pattern = $"{name}:{serviceId}";

            return pattern.Equals(consulRegisteredName);
        }
    }
}
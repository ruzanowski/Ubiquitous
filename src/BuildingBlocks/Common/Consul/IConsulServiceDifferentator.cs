using U.Common.Mvc;

namespace U.Common.Consul
{
    public interface IConsulServiceDifferentator
    {
        bool IsTheSame(string consulRegisteredName);
    }

    public class ConsulServiceDiffentator : IConsulServiceDifferentator
    {
        private readonly IServiceIdService _idService;
        private readonly ConsulOptions _consulOptions;

        public ConsulServiceDiffentator(IServiceIdService idService, ConsulOptions consulOptions)
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
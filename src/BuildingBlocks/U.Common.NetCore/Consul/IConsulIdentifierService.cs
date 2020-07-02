using U.Common.NetCore.Mvc;

namespace U.Common.NetCore.Consul
{
    public interface IConsulIdentifierService
    {
        bool IsTheSame(string id);
    }

    public class ConsulIdentifierIdentifierService : IConsulIdentifierService
    {
        private readonly ISelfInfoService _idService;

        public ConsulIdentifierIdentifierService(ISelfInfoService idService)
        {
            _idService = idService;
        }

        public bool IsTheSame(string id)
        {
            var serviceId = _idService.Id;
            var pattern = $"{serviceId}";

            return pattern.Equals(id);
        }
    }
}
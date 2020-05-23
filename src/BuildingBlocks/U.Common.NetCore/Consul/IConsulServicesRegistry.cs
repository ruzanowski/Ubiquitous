using System.Threading.Tasks;
using Consul;

namespace U.Common.NetCore.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string name);
    }
}
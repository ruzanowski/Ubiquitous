using System.Threading.Tasks;
using Consul;

namespace U.Common.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string name);
    }
}
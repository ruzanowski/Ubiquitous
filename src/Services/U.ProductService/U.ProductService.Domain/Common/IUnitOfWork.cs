using System;
using System.Threading;
using System.Threading.Tasks;

namespace U.ProductService.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}

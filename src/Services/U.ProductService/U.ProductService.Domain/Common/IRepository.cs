using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Common
{
    public interface IRepository<T> where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}

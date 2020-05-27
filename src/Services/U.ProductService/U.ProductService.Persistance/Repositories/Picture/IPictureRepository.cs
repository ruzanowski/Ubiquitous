using System;
using System.Threading.Tasks;
using U.ProductService.Domain.Common;

namespace U.ProductService.Persistance.Repositories.Picture
{
    public interface IPictureRepository : IRepository<Domain.Entities.Picture.Picture>
    {
        Task<Domain.Entities.Picture.Picture> AddAsync(Domain.Entities.Picture.Picture picture);
        void Update(Domain.Entities.Picture.Picture picture);
        Task<Domain.Entities.Picture.Picture> GetAsync(Guid pictureId);
        Task Delete(Guid pictureId);
    }
}
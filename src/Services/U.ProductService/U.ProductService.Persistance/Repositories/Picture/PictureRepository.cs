using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Contexts;
using Z.EntityFramework.Plus;

namespace U.ProductService.Persistance.Repositories.Picture
{
    public class PictureRepository : IPictureRepository
    {
        private readonly ProductContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public PictureRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Domain.Entities.Picture.Picture> AddAsync(Domain.Entities.Picture.Picture picture)
        {
            return (await _context.Pictures.AddAsync(picture)).Entity;
        }

        public async Task<Domain.Entities.Picture.Picture> GetAsync(Guid pictureId)
        {
            var picture = await _context.Pictures
                .Include(x => x.MimeType)
                .FirstOrDefaultAsync(x => x.Id.Equals(pictureId));

            return picture;
        }

        public async Task Delete(Guid pictureId)
        {
            var picture = _context.Pictures.First(x => x.Id.Equals(pictureId));

            _context.Pictures.Remove(picture);
        }

        public void Update(Domain.Entities.Picture.Picture picture)
        {
            _context.Update(picture);
        }
    }
}
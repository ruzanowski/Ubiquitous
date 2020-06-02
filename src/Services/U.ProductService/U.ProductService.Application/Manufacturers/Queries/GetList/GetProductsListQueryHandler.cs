using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.Common.Pagination;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Manufacturers.Queries.GetList
{
    public class GetProductsListQueryHandler : IRequestHandler<GetManufacturersListQuery, PaginatedItems<ManufacturerViewModel>>
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<ManufacturerViewModel>> Handle(GetManufacturersListQuery request, CancellationToken cancellationToken)
        {
            var products = GetProductQueryable();

            var manufacturersMapped = _mapper.ProjectTo<ManufacturerViewModel>(products);

            var paginatedProducts = PaginatedItems<ManufacturerViewModel>.Create(request.PageIndex,
                    request.PageSize, manufacturersMapped);

            await Task.CompletedTask;

            return paginatedProducts;
        }

        private IQueryable<Manufacturer> GetProductQueryable() => _context.Manufacturers
            .Include(x => x.Pictures)
            .AsQueryable();
    }
}
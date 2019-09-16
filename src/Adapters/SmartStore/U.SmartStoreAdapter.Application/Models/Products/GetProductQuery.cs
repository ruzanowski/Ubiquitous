using MediatR;

namespace U.SmartStoreAdapter.Application.Models.Products
{
    public class GetProductQuery : IRequest<SmartProductViewModel>
    {
        public int Id { get; set; }
        public string Sku { get; set; }
    }
}
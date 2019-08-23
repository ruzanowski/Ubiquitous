using MediatR;

namespace U.SmartStoreAdapter.Api.Products
{
    public class GetProductQuery : IRequest<SmartProductViewModel>
    {
        public int Id { get; set; }
        public string Sku { get; set; }
    }
}
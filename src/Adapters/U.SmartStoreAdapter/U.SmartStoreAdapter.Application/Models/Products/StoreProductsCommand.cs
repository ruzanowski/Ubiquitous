using MediatR;

namespace U.SmartStoreAdapter.Application.Models.Products
{
    public class StoreProductsCommand : SmartProductDto, IRequest<int>
    {
    }
}    
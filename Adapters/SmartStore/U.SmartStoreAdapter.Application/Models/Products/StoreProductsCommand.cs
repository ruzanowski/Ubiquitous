using MediatR;

namespace U.SmartStoreAdapter.Api.Products
{
    public class StoreProductsCommand : SmartProductDto, IRequest<int>
    {
    }
}    
using MediatR;

namespace U.SmartStoreAdapter.Application.Products
{
    public class StoreProductsCommand : SmartProductDto, IRequest<int>
    {
    }
}    
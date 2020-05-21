using MediatR;
using U.Common.Products;

namespace U.SmartStoreAdapter.Application.Products
{
    public class StoreProductsCommand : SmartProductDto, IRequest<int>
    {
    }
}    
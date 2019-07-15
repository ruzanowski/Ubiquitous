using MediatR;
using U.Common;

namespace U.SmartStoreAdapter.Api.Products
{
    public class StoreProductsCommand : SmartProductDto, IRequest<DataTransaction<SmartProductDto, SmartProductDto>>
    {
    }
}
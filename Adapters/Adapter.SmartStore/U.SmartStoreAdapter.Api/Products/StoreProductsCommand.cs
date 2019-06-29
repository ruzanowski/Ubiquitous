using MediatR;
using U.SmartStoreAdapter.Application.Models.DataRequests;

namespace U.SmartStoreAdapter.Api.Products
{
    public class StoreProductsCommand : SmartProductDto, IRequest<DataTransaction<SmartProductDto, SmartProductDto>>
    {
    }
}
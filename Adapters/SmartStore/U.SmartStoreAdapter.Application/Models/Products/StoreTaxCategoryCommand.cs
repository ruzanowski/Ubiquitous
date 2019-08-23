using MediatR;
using U.SmartStoreAdapter.Api.Taxes;

namespace U.SmartStoreAdapter.Api.Products
{
    public class StoreTaxCategoryCommand : TaxCategoryDto, IRequest<TaxResponse>
    {
    }
}
using MediatR;
using U.SmartStoreAdapter.Application.Models.Taxes;

namespace U.SmartStoreAdapter.Application.Models.Products
{
    public class StoreTaxCategoryCommand : TaxCategoryDto, IRequest<TaxResponse>
    {
    }
}
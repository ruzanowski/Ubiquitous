using MediatR;

namespace U.SmartStoreAdapter.Api.Categories
{
    public class StoreCategoryCommand : CategoryDto, IRequest<int>
    {
        
    }
}
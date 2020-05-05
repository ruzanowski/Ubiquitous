using MediatR;

namespace U.SmartStoreAdapter.Application.Categories
{
    public class StoreCategoryCommand : CategoryDto, IRequest<int>
    {
        
    }
}
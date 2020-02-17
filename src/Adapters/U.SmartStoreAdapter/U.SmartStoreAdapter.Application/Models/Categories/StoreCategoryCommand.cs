using MediatR;

namespace U.SmartStoreAdapter.Application.Models.Categories
{
    public class StoreCategoryCommand : CategoryDto, IRequest<int>
    {
        
    }
}
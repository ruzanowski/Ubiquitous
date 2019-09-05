using MediatR;

namespace U.SmartStoreAdapter.Application.Models.Pictures
{
    public class StorePicturesCommand : PictureDto, IRequest<int>
    {
        
    }
}
using MediatR;

namespace U.SmartStoreAdapter.Api.Pictures
{
    public class StorePicturesCommand : PictureDto, IRequest<int>
    {
        
    }
}
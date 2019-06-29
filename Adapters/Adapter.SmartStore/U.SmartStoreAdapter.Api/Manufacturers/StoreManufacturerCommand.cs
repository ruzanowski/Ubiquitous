using MediatR;

namespace U.SmartStoreAdapter.Api.Manufacturers
{
    public class StoreManufacturerCommand :  IRequest<ManufacturerViewModel>
    {
        public ManufacturerDto ManufacturerDto { get; set; }
    }
}
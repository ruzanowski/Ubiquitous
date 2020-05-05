using MediatR;

namespace U.SmartStoreAdapter.Application.Manufacturers
{
    public class StoreManufacturerCommand :  IRequest<ManufacturerViewModel>
    {
        public ManufacturerDto ManufacturerDto { get; set; }
    }
}
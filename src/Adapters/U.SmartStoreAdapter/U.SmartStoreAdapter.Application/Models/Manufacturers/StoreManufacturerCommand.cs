using MediatR;

namespace U.SmartStoreAdapter.Application.Models.Manufacturers
{
    public class StoreManufacturerCommand :  IRequest<ManufacturerViewModel>
    {
        public ManufacturerDto ManufacturerDto { get; set; }
    }
}
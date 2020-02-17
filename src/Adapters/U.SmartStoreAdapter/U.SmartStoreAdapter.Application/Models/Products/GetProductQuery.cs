using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace U.SmartStoreAdapter.Application.Models.Products
{
    public class GetProductQuery : IRequest<SmartProductViewModel>
    {
        [FromRoute]
        public int Id { get; set; }
        public string Sku { get; set; }
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Products;

namespace U.SmartStoreAdapter.Application.Products
{
    public class GetProductQuery : IRequest<SmartProductViewModel>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
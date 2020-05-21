using System;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetSingleByAlternativeKey
{

    /// <summary>
    /// Alternative key is: WHOLESALE.MANUFACTURER.BARCODE
    /// </summary>
    public class QueryProductByAlternativeKey : IRequest<ProductViewModel>
    {
        public string ExternalSourceName { get; set; }
        public string ExternalSourceId { get; set; }
    }
}
using System.Collections.Generic;

namespace U.FetchService.Application.Models.Wholesales
{
    public class AvailableWholesales : IAvailableWholesales
    {
        public IEnumerable<IWholesale> Wholesales { get; set; }
    }
}
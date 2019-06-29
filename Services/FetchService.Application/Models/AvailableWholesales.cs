using System.Collections.Generic;

namespace U.FetchService.Application.Models.AvailableWholesales
{
    public class AvailableWholesales : IAvailableWholesales
    {
        public IEnumerable<IWholesale> Wholesales { get; set; }
    }
}
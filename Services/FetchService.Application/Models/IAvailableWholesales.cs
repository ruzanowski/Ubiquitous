using System.Collections.Generic;

namespace U.FetchService.Application.Models.AvailableWholesales
{
    public interface IAvailableWholesales
    {
        IEnumerable<IWholesale> Wholesales { get; set; }
    }
}
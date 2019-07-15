using System.Collections.Generic;

namespace U.FetchService.Application.Models.Wholesales
{
    public interface IAvailableWholesales
    {
        IEnumerable<IWholesale> Wholesales { get; set; }
    }
}
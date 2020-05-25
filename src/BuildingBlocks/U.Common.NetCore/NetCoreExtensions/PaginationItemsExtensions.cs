using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.Pagination;

namespace U.Common.NetCore.NetCoreExtensions
{
    public class PaginatedItemsExtended<T> : PaginatedItems<T>
    {
        public static async Task<PaginatedItems<T>> CreateAsync(int pageIndex, int pageSize, IQueryable<T> data)
        {
            return new PaginatedItems<T>(pageIndex,
                pageSize,
                await data.Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync());
        }

        public PaginatedItemsExtended(int pageIndex, int pageSize, IEnumerable<T> data) : base(pageIndex, pageSize, data)
        {
        }
    }
}
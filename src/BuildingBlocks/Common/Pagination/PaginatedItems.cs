using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace U.Common.Pagination
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PaginatedItems<T> : IPagination
    {    
        public PaginatedItems(int pageIndex,int pageSize, IEnumerable<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
        }
        public IEnumerable<T> Data { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

            public static async Task<PaginatedItems<T>> CreateAsync(int pageIndex, int pageSize, IQueryable<T> data)
            {
                return new PaginatedItems<T>(pageIndex,
                    pageSize,
                    await data.Skip(pageIndex * pageSize) 
                        .Take(pageSize)
                        .ToListAsync());
            }
    }
}
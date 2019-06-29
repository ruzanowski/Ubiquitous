using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace U.FetchService.Application.Models
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
        public IEnumerable<T> Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public static class PaginatedItemsCreate
        {
            public static async Task<PaginatedItems<T>> CreateAsync(int pageIndex, int pageSize, IQueryable<T> data)
            {
                return new PaginatedItems<T>(pageIndex,
                    pageSize,
                    await data
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync());
            }
        }
    }
}
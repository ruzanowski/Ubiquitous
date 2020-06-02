using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;

namespace U.Common.Pagination
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PaginatedItems<T> : IPagination
    {
        private PaginatedItems()
        {

        }

        public PaginatedItems(int pageIndex, int pageSize, IEnumerable<T> data) : this()
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
        }

        public IEnumerable<T> Data { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

        public static PaginatedItems<T> Create(int pageIndex, int pageSize, IQueryable<T> data)
        {
            return new PaginatedItems<T>(pageIndex,
                pageSize,
                data.Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToList());
        }
    }
}
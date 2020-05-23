using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace U.Common.Pagination
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PaginatedItems<T> : IPagination
    {
        public PaginatedItems(int pageIndex, int pageSize, IEnumerable<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
        }

        public IEnumerable<T> Data { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
    }
}
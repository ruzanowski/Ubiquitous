using System.Diagnostics.CodeAnalysis;

namespace U.Common.Pagination
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IPagination
    {
        int PageIndex { get;  }
        int PageSize { get; }
    }
}
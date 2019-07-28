using System.Diagnostics.CodeAnalysis;

namespace U.Common.Pagination
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IPagination
    {
        int PageIndex { get;  set; }
        int PageSize { get; set; }
    }
}
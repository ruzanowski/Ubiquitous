using System.Diagnostics.CodeAnalysis;

namespace U.Common
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IPagination
    {
        int PageIndex { get;  set; }
        int PageSize { get; set; }
    }
}
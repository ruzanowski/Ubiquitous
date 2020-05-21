using System.Diagnostics.CodeAnalysis;

namespace U.Common.Products
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SmartProductViewModel : SmartProductDto
    {
        public new string Id { get; set; }
    }
}
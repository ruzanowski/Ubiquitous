using System.Collections.Generic;
using U.ProductService.Domain.SeedWork;

// ReSharper disable CheckNamespace

namespace U.ProductService.Domain
{
    public class Dimensions : ValueObject
    {
        public decimal Length { get; protected internal set; }
        public decimal Width { get; protected internal set; }
        public decimal Height { get; protected internal set; }
        public decimal Weight { get; protected internal set; }
        
        public Dimensions(decimal length, decimal width, decimal height, decimal weight)
        {
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Length;
            yield return Width;
            yield return Height;
            yield return Weight;
        }
    }
}
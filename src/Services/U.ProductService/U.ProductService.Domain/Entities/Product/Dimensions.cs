using System.Collections.Generic;
using U.ProductService.Domain.Common;

// ReSharper disable CheckNamespace

namespace U.ProductService.Domain
{
    public class Dimensions : ValueObject
    {
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        private Dimensions()
        {

        }

        public Dimensions(decimal length, decimal width, decimal height, decimal weight) : this()
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
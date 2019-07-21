using System.Collections.Generic;
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain.Aggregates.Product
{
    public class Dimensions : ValueObject
    {
        public decimal Length { get; private set; }
        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Weight { get; private set; }

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
using System.Collections.Generic;
using System.Reflection;

namespace U.ProductService.Domain.Helpers
{
    public static class ComparerExtensions
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            var variances = new List<Variance>();
            FieldInfo[] fieldInfos = val1.GetType().GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                var variance = new Variance();
                variance.Prop = fieldInfo.Name;
                variance.ValueA = fieldInfo.GetValue(val1);
                variance.ValueB = fieldInfo.GetValue(val2);
                if (!variance.ValueA.Equals(variance.ValueB))
                    variances.Add(variance);
            }

            return variances;
        }

        public static List<Variance> ExamineProductVariances(this Product product, Product product2)
        {
            var variances = product.DetailedCompare(product2);
            variances.AddRange(product.Dimensions.DetailedCompare(product2.Dimensions));
            variances.AddRange(product.Category.DetailedCompare(product2.Category));
            variances.AddRange(product.Pictures.DetailedCompare(product2.Pictures));

            return variances;
        }
    }
}
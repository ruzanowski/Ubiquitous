using System.Collections.Generic;
using System.Reflection;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Update
{
    public static class ComparerExtensions
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            List<Variance> variances = new List<Variance>();
            FieldInfo[] fi = val1.GetType().GetFields();
            foreach (FieldInfo f in fi)
            {
                Variance variance = new Variance();
                variance.Prop = f.Name;
                variance.ValueA = f.GetValue(val1);
                variance.ValueB = f.GetValue(val2);
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

    public class Variance
    {
        public string Prop { get; set; }
        public object ValueA { get; set; }
        public object ValueB { get; set; }
    }
}
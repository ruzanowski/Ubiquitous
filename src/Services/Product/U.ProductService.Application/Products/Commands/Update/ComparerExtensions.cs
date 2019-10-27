using System.Collections.Generic;
using System.Reflection;

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
    }

    public class Variance
    {
        public string Prop { get; set; }
        public object ValueA { get; set; }
        public object ValueB { get; set; }
    }
}
using System;

namespace U.ProductService.Application.Products.Models
{
    public class DimensionsDto : IEquatable<DimensionsDto>
    {
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        public bool Equals(DimensionsDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Length == other.Length && Width == other.Width && Height == other.Height && Weight == other.Weight;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DimensionsDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Length, Width, Height, Weight);
        }
    }
}
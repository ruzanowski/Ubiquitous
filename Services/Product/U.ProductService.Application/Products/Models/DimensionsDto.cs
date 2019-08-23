namespace U.ProductService.Application.Products.Models
{
    public class DimensionsDto
    {
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        public DimensionsDto()
        {
            
        }
        
        public DimensionsDto(decimal length, decimal width, decimal height, decimal weight)
        {
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
        }
    }
}
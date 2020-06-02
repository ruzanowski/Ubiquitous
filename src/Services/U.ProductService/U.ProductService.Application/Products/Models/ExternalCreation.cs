namespace U.ProductService.Application.Products.Models
{
    public class ExternalCreation
    {
        public string SourceName { get; set; }
        public string SourceId { get; set; }
        public bool DuplicationValidated { get; set; }
    }
}
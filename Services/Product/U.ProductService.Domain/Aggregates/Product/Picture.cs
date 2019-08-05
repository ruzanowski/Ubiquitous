using U.ProductService.Domain.SeedWork;
// ReSharper disable CheckNamespace

namespace U.ProductService.Domain.Aggregates
{

    /// <summary>
    /// Picture entity
    /// </summary>
    public class Picture : Entity
    {
        /// <summary>
        /// Gets or sets the SEO friednly filename of the picture
        /// </summary>
        public string SeoFilename { get; private set; }
        
        /// <summary>
        /// Gets or sets the SEO friednly filename of the picture
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Gets or sets the SEO friednly filename of the picture
        /// </summary>
        public string Url { get; private set; }
        
        /// <summary>
        /// Gets or sets the picture mime type
        /// </summary>
        public string MimeType { get; private set; }

        public Picture(string seoFilename, string description, string url, string mimeType)
        {    
            SeoFilename = seoFilename;
            Description = description;
            Url = url;
            MimeType = mimeType;
        }
    }
}
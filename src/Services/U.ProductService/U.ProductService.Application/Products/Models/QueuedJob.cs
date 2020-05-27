using System;

namespace U.ProductService.Application.Products.Models
{
    public class QueuedJob
    {
        public bool AutoSave { get; set; } = false;
        public DateTime DateTimeQueued { get; set; } = DateTime.UtcNow;
    }
}
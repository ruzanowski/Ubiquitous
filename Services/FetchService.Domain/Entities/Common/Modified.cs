using System;

namespace U.FetchService.Domain.Entities.Common
{
    public class Modified
    {
        public int Id { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }
    }
}
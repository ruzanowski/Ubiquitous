using System;

namespace U.NotificationService.Domain.Entities
{
    public class UserBasedEventImportancy
    {
        public int Id { get; set; }
        public Importancy Importancy { get; set; }
        public Guid UserId { get; set; }
    }
}
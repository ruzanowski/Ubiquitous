using System;

namespace U.NotificationService.Domain
{
    public class Confirmation
    {
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public Guid NotificationId { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public ConfirmationType ConfirmationType { get; set; }
        public Guid DeviceReceivedId { get; set; }
    }

    public enum ConfirmationType
    {
        Unread,
        Read,
        Removed,
        Hidden
    }
}
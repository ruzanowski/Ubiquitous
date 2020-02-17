using System;

namespace U.NotificationService.Domain.Entities
{
    public class Confirmation
    {
        //necessary for ef migrations
        private Confirmation()
        {

        }

        public Confirmation(Guid user, Guid notificationId, ConfirmationType confirmationType, Guid deviceReceivedId)
        {
            User = user;
            NotificationId = notificationId;
            ConfirmationDate = DateTime.UtcNow;
            ConfirmationType = confirmationType;
            DeviceReceivedId = deviceReceivedId;
        }

        public Guid Id { get; private set; }
        public Guid User { get; private set; }
        public Guid NotificationId { get; private set; }
        public DateTime ConfirmationDate { get; private set; }
        public ConfirmationType ConfirmationType { get; private set; }
        public Guid DeviceReceivedId { get; private set; }
    }

    public enum ConfirmationType
    {
        Unread,
        Read,
        Hidden
    }
}
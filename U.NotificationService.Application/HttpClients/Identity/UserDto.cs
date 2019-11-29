using System;

namespace U.NotificationService.Application.HttpClients.Identity
{
    public class UserDto
    {

        public Guid Id { get;  set; }
        public string Email { get;  set; }
        public string Nickname { get; set; }
        public string Role { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime UpdatedAt { get;  set; }
    }
}
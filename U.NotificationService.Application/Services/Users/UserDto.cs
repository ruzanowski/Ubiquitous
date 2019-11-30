using System;

namespace U.NotificationService.Application.Services.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
        public string Email { get;  set; }
    }
}
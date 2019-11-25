using System;

namespace U.IdentityService.Application.Models
{
    public class UserDto
    {

        public Guid Id { get;  set; }
        public string Email { get;  set; }
        public string Role { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime UpdatedAt { get;  set; }
    }
}
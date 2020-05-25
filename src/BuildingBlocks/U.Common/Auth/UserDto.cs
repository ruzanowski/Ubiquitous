using System;

namespace U.Common.Auth
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
        public string Email { get;  set; }
        public string AccessToken { get;  set; }

    }
}
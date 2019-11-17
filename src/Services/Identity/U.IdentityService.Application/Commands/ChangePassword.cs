using System;
using Newtonsoft.Json;

namespace U.IdentityService.Application.Commands
{
    public class ChangePassword
    {
        public Guid UserId { get; }
        public string CurrentPassword { get; }
        public string NewPassword { get; }

        [JsonConstructor]
        public ChangePassword(Guid userId,
            string currentPassword,string newPassword)
        {
            UserId = userId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}
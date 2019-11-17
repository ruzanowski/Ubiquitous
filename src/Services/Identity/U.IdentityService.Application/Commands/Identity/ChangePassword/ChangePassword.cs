using System;
using MediatR;

namespace U.IdentityService.Application.Commands.Identity.ChangePassword
{
    public class ChangePassword : ChangePasswordDto, IRequest<Unit>
    {
        public Guid UserId { get; set; }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
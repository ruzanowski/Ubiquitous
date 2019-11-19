using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace U.IdentityService.Application.Commands.Identity.ChangePassword
{
    public class ChangePassword : ChangePasswordDto, IRequest<Unit>
    {
        public Guid UserId { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
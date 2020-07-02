using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace U.IdentityService.Application.Commands.Identity.SignUp
{
    public class SignUp : IRequest<Unit>
    {
        internal Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Nickname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
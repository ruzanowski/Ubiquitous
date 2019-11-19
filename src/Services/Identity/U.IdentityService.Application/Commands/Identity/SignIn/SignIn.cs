using System.ComponentModel.DataAnnotations;
using MediatR;
using U.Common.Jwt;

namespace U.IdentityService.Application.Commands.Identity.SignIn
{
    public class SignIn : IRequest<JsonWebToken>
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
using System;
using MediatR;

namespace U.IdentityService.Application.Commands.Identity.SignUp
{
    public class SignUp : IRequest<Unit>
    {
        internal Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string Password { get; set;  }
        public string Role { get; set; }

    }
}
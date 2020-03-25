using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Jam_Inspired_Application.ErrorHandling;
using Jam_Inspired_Application.Interfaces;
using Jam_Inspired_Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Jam_Inspired_Application.User
{
    public class Login
    {
        public class Query : IRequest<User>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenrator;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenrator)
            {
                _jwtGenrator = jwtGenrator;
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    throw new RestExceptionHandling(HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenrator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };
                }

                throw new RestExceptionHandling(HttpStatusCode.Unauthorized);
            }
        }
    }
}

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Jam_Inspired_Application.ErrorHandling;
using Jam_Inspired_Application.Interfaces;
using Jam_Inspired_Application.Validators;
using Jam_Inspired_Domain;
using Jam_Inspired_Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jam_Inspired_Application.User
{
    public class Register
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).EmailAddress().NotEmpty();
                RuleFor(x => x.Password).Password();
            }
        }
        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _context = context;
                _userManager = userManager;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                //TODO : handler logic
                if(await _context.Users.Where(x => x.Email == request.Email).AnyAsync()) {
                    throw new RestExceptionHandling(HttpStatusCode.BadRequest, new {Email = "Email already exist!"});
                }

                if(await _context.Users.Where(x => x.UserName == request.Username).AnyAsync()) {
                    throw new RestExceptionHandling(HttpStatusCode.BadRequest, new {Username = "Username already exist!"});
                }

                var user = new AppUser {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.Username
                };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded) {
                    return new User {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };
                }

                throw new Exception("Could not create user...");
            }
        }
    }
}
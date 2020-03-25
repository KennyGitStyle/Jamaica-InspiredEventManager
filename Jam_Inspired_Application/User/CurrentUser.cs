using System.Threading;
using System.Threading.Tasks;
using Jam_Inspired_Application.Interfaces;
using Jam_Inspired_Domain;
using Jam_Inspired_Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Jam_Inspired_Application.User
{
    public class CurrentUser
    {
        public class Query : IRequest<User> { }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;

            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;

            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return new User {
                  DisplayName = user.DisplayName,
                  Username = user.UserName,
                  Token = _jwtGenerator.CreateToken(user),
                  Image = null
                };
            }
        }
    }
}

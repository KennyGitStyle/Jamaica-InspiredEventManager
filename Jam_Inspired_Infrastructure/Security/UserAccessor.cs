using System.Security.Claims;
using System.Linq;
using Jam_Inspired_Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Jam_Inspired_Infrastructure.Security {
    public class UserAccessor : IUserAccessor {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor (IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername () {
            var username = _httpContextAccessor
            .HttpContext.User?.Claims?
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return username;
        }
    }
}
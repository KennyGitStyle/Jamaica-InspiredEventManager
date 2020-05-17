using System.Threading.Tasks;
using Jam_Inspired_Application.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jam_Inspired.API.Controllers
{

    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Login.Query loginQuery) => await Mediator.Send(loginQuery);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Register.Command registerCommand) => await Mediator.Send(registerCommand);

        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser() => await Mediator.Send(new CurrentUser.Query());
    }

}
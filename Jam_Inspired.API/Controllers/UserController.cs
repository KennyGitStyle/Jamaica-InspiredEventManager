using System.Threading.Tasks;
using Jam_Inspired_Application.User;
using Jam_Inspired_Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jam_Inspired.API.Controllers
{
    
    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Login.Query loginQuery){
            var userLogin = await Mediator.Send(loginQuery);
            return userLogin;
        }

        [AllowAnonymous]
        [HttpPost("register")]
         public async Task<ActionResult<User>> Register(Register.Command registerCommand){
             var userRegistration = await Mediator.Send(registerCommand);
             return userRegistration;
         }

         [HttpGet]
         public async Task<ActionResult<User>> CurrentUser(){
             var currentUser = await Mediator.Send(new CurrentUser.Query());
             return currentUser;
         }
    }

}
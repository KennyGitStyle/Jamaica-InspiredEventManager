using System.Threading.Tasks;
using Jam_Inspired_Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace Jam_Inspired.API.Controllers
{
    public class ProfilesController : BaseController
    {
        [HttpGet("{username}")]
        public async Task<ActionResult<Profile>> Get(string username) => await Mediator.Send(new Details.Query { Username = username });
    }
}
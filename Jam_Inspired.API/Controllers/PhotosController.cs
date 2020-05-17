using System.Threading.Tasks;
using Jam_Inspired_Application.Photos;
using Jam_Inspired_Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jam_Inspired.API.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm]Add.Command command) => await Mediator.Send(command);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id) => await Mediator.Send(new Delete.Command{Id = id});

        [HttpPost("{id}/setmain")]
        public async Task<ActionResult<Unit>> SetMain(string id) => await Mediator.Send(new SetMain.Command{Id = id});
    }
}
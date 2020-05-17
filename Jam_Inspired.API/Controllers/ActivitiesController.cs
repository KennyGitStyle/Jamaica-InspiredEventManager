using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam_Inspired_Application.Activities;
using Jam_Inspired_Application.Activities.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jam_Inspired.API.Controllers
{
    public class ActivitiesController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List() => await Mediator.Send(new List.Query());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ActivityDto>> Details(Guid id) => await Mediator.Send(new Details.Query { Id = id });

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command) => await Mediator.Send(command);

        [HttpPut("{id}")]
        [Authorize(Policy ="IsActivityHost")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult<Unit>> Delete(Guid id) => await Mediator.Send(new Delete.Command { Id = id });

        [HttpPost("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend(Guid id) => await Mediator.Send(new Attend.Command { Id = id });

        [HttpDelete("{id}/attend")]
        public async Task<ActionResult<Unit>> Unattend(Guid id) => await Mediator.Send(new Unattend.Command { Id = id });

    }
}
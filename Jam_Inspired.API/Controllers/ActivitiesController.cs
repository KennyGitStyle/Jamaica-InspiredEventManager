using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam_Inspired_Application.Activities;
using Jam_Inspired_Application.Activities.Dto;
using Jam_Inspired_Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jam_Inspired.API.Controllers
{
    public class ActivitiesController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List()
        {
            var getActivities = await Mediator.Send(new List.Query());
            return getActivities;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ActivityDto>> Details(Guid id){
            var activity = await Mediator.Send(new Details.Query{Id = id});
            return activity;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
           var createActivity = await Mediator.Send(command);

           return createActivity;
        }

        [HttpPut("{id}")]
        [Authorize(Policy ="IsActivityHost")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            var editActivity = await Mediator.Send(command);

            return editActivity;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="IsActivityHost")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            var deleteActivity = await Mediator.Send(new Delete.Command{Id = id});

            return deleteActivity;
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend(Guid id) {

            var attendActivity = await Mediator.Send(new Attend.Command{Id = id});

            return attendActivity;
        }

        [HttpDelete("{id}/attend")]
        public async Task<ActionResult<Unit>> Unattend(Guid id) {
            var unattendActivity = await Mediator.Send(new Unattend.Command{Id = id});

            return unattendActivity;
        }

    }
}
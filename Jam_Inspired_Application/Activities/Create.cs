using System.Data.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using Jam_Inspired_Domain;
using Jam_Inspired_Persistence;
using MediatR;
using FluentValidation;
using Jam_Inspired_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jam_Inspired_Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class CommandValidatior : AbstractValidator<Command>
        {
            public CommandValidatior()
            {
                RuleFor(x => x.Title).NotEmpty().MinimumLength(5);
                RuleFor(x => x.Description).NotEmpty().MinimumLength(5);
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x=> x.Date.AddHours(24)).NotEmpty();
                RuleFor(x=> x.City).NotEmpty();
                RuleFor(x=> x.Venue).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = new Activity 
                {
                   Id = request.Id,
                   Title = request.Title,
                   Description = request.Description,
                   Category = request.Category,
                   Date = request.Date,
                   City = request.City,
                   Venue = request.Venue
                };

                 _context.Activities.Add(activity);

                 var user = await _context.Users.SingleOrDefaultAsync(u => 
                    u.UserName == _userAccessor.GetCurrentUsername());

                var attendee = new UserActivity
                {
                    AppUser = user,
                    Activity = activity,
                    IsHost = true,
                    DateJoined = DateTime.Now
                };

                _context.UserActivities.Add(attendee);
                
                 var successfullyAdded = await _context.SaveChangesAsync() > 0;

                 if(successfullyAdded) return Unit.Value;

                 throw new Exception("Change has not been saved...");

            }
        }
    }
}
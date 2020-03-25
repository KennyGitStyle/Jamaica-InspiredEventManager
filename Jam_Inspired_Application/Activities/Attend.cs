using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Jam_Inspired_Application.ErrorHandling;
using Jam_Inspired_Application.Interfaces;
using Jam_Inspired_Domain;
using Jam_Inspired_Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jam_Inspired_Application.Activities
{
    public class Attend
    {
        public class Command : IRequest 
        {
            public Guid Id { get; set; }
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

            public async Task<Unit>Handle(Command request, CancellationToken cancellationToken)
            {

                var activity = await _context.Activities.FindAsync(request.Id);

                if(activity == null) throw new RestExceptionHandling(HttpStatusCode.NotFound, new {Activity = "Could not find activity"});

                var user = await _context.Users.SingleOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetCurrentUsername());

                var attendance = await _context.UserActivities.SingleOrDefaultAsync(x => 
                    x.ActivityId == activity.Id && x.AppUserId == user.Id);

                if(attendance != null) {
                    throw new RestExceptionHandling(HttpStatusCode.BadRequest, 
                    new {Attendance = "Already attending activity..."});
                }

                attendance = new UserActivity {
                    Activity = activity,
                    AppUser = user,
                    IsHost = false,
                    DateJoined = DateTime.Now
                };

                _context.UserActivities.Add(attendance);

                var successfullyAdded = await _context.SaveChangesAsync() > 0;

                if (successfullyAdded) return Unit.Value;

                throw new Exception("Change has not been saved...");
            }
        }
    }
}

using System.Diagnostics;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Jam_Inspired_Application.ErrorHandling;
using Jam_Inspired_Persistence;
using MediatR;

namespace Jam_Inspired_Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var deleteActivity  = await _context.Activities.FindAsync(request.Id);

                if(deleteActivity == null) throw new RestExceptionHandling(HttpStatusCode.Found, new {deleteActivity = "Does not exist!" });

                _context.Remove(deleteActivity);

                var successfullyAdded = await _context.SaveChangesAsync() > 0;

                if (successfullyAdded) return Unit.Value;

                throw new Exception("Change has not been saved...");

            }
        }
    }
}
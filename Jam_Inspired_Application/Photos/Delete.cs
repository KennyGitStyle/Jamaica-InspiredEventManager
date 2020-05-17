using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Jam_Inspired_Application.Interfaces;
using Jam_Inspired_Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Jam_Inspired_Application.ErrorHandling;
using System.Net;

namespace Jam_Inspired_Application.Photos
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _photoAccessor = photoAccessor;
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit>
            Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                if(photo == null){
                    throw new RestExceptionHandling(HttpStatusCode.NotFound, new {Photo = "Not found"});
                }

                if(photo.IsMain){
                    throw new RestExceptionHandling(HttpStatusCode.BadRequest, new{Photo = "You cannot delete your main photo"});
                }

                var result = _photoAccessor.DeletePhoto(photo.Id);

                if(result == null){
                    throw new Exception("Problem deleting photo..");
                }

                user.Photos.Remove(photo);

                var successfullyAdded = await _context.SaveChangesAsync() > 0;

                if (successfullyAdded) return Unit.Value;

                throw new Exception("Change has not been saved...");
            }
        }
    }
}

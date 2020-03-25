using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Jam_Inspired_Application.Activities.Dto;
using Jam_Inspired_Application.ErrorHandling;
using Jam_Inspired_Domain;
using Jam_Inspired_Persistence;
using MediatR;

namespace Jam_Inspired_Application.Activities
{
    public class Details
    {
        public class Query : IRequest<ActivityDto> 
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);
                if(activity == null) throw new RestExceptionHandling(HttpStatusCode.Found, new {activity = "Does not exist!" });
                
                var activityMapToDto = _mapper.Map<Activity, ActivityDto>(activity);
                
                return activityMapToDto;
            }
        }


    }
}
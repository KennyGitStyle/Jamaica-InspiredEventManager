using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Jam_Inspired_Application.Activities.Dto;
using Jam_Inspired_Domain;
using Jam_Inspired_Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jam_Inspired_Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<ActivityDto>> { }

        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activityList = await _context.Activities.ToListAsync(); 
                var activityListMapToDto = _mapper.Map<List<Activity>, List<ActivityDto>>(activityList);
                return activityListMapToDto;
            }
        }
    }
}
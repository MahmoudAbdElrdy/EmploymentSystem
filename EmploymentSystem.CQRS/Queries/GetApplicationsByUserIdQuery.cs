using EmploymentSystem.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.CQRS.Queries
{
    public class GetApplicationsByUserIdQuery : IRequest<List<Application>>
    {
        public int UserId { get; set; }
    }

    public class GetApplicationsByUserIdQueryHandler : IRequestHandler<GetApplicationsByUserIdQuery, List<Application>>
    {
        private readonly ApplicationDbContext _context;

        public GetApplicationsByUserIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Application>> Handle(GetApplicationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Applications.Where(a => a.UserId == request.UserId).ToListAsync();
        }
    }

}

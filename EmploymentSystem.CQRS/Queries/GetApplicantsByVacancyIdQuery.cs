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
    public class GetApplicantsByVacancyIdQuery : IRequest<List<User>>
    {
        public int VacancyId { get; set; }
    }

    public class GetApplicantsByVacancyIdQueryHandler : IRequestHandler<GetApplicantsByVacancyIdQuery, List<User>>
    {
        private readonly ApplicationDbContext _context;

        public GetApplicantsByVacancyIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Handle(GetApplicantsByVacancyIdQuery request, CancellationToken cancellationToken)
        {
            var applications = await _context.Applications
                .Where(a => a.VacancyId == request.VacancyId)
                .Select(a => a.UserId)
                .ToListAsync();

            return await _context.Users.Where(u => applications.Contains(u.Id)).ToListAsync();
        }
    }

}

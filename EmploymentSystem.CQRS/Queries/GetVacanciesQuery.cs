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
    public class GetVacanciesQuery : IRequest<List<Vacancy>>
    {
    }

    public class GetVacanciesQueryHandler : IRequestHandler<GetVacanciesQuery, List<Vacancy>>
    {
        private readonly ApplicationDbContext _context;

        public GetVacanciesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vacancy>> Handle(GetVacanciesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Vacancies.Where(v => v.IsActive && v.ExpiryDate > DateTime.Now).ToListAsync();
        }
    }

}

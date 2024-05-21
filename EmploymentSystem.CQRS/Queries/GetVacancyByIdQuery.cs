using EmploymentSystem.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.CQRS.Queries
{
    public class GetVacancyByIdQuery : IRequest<Vacancy>
    {
        public int Id { get; set; }
    }

    public class GetVacancyByIdQueryHandler : IRequestHandler<GetVacancyByIdQuery, Vacancy>
    {
        private readonly ApplicationDbContext _context;

        public GetVacancyByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vacancy> Handle(GetVacancyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Vacancies.FindAsync(request.Id);
        }
    }

}

using EmploymentSystem.Data;
using EmploymentSystem.Services.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.CQRS.Commands
{
    public class CreateVacancyCommand :  IRequest<Unit>
    {
        public VacancyDto Vacancy { get; set; }
    }

    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateVacancyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
        {
            var vacancy = new Vacancy
            {
                Title = request.Vacancy.Title,
                Description = request.Vacancy.Description,
                ExpiryDate = request.Vacancy.ExpiryDate,
                MaxApplications = request.Vacancy.MaxApplications,
                IsActive = true
            };

            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

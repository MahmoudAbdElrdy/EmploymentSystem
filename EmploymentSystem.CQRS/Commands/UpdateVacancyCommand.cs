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
    public class UpdateVacancyCommand : IRequest
    {
        public int Id { get; set; }
        public VacancyDto Vacancy { get; set; }
    }

    public class UpdateVacancyCommandHandler : IRequestHandler<UpdateVacancyCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdateVacancyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateVacancyCommand request, CancellationToken cancellationToken)
        {
            var vacancy = await _context.Vacancies.FindAsync(request.Id);
            if (vacancy == null)
            {
                throw new  Exception("Vacancy not found");
            }

            vacancy.Title = request.Vacancy.Title;
            vacancy.Description = request.Vacancy.Description;
            vacancy.ExpiryDate = request.Vacancy.ExpiryDate;
            vacancy.MaxApplications = request.Vacancy.MaxApplications;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

using EmploymentSystem.Data;
using EmploymentSystem.Services.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.CQRS.Commands
{
    public class ApplyForVacancyCommand : IRequest
    {
        public ApplicationDto Application { get; set; }
    }

    public class ApplyForVacancyCommandHandler : IRequestHandler<ApplyForVacancyCommand>
    {
        private readonly ApplicationDbContext _context;

        public ApplyForVacancyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ApplyForVacancyCommand request, CancellationToken cancellationToken)
        {
            var vacancy = await _context.Vacancies.FindAsync(request.Application.VacancyId);
            if (vacancy == null || !vacancy.IsActive || vacancy.ExpiryDate < DateTime.Now)
            {
                throw new InvalidOperationException("Vacancy is not available for application");
            }

            var applicationCount = await _context.Applications.CountAsync(a => a.VacancyId == request.Application.VacancyId);
            if (applicationCount >= vacancy.MaxApplications)
            {
                throw new InvalidOperationException("Maximum number of applications reached for this vacancy");
            }

            var userApplications = await _context.Applications
                .Where(a => a.UserId == request.Application.UserId && a.AppliedDate > DateTime.Now.AddDays(-1))
                .CountAsync();
            if (userApplications > 0)
            {
                throw new InvalidOperationException("User can only apply to one vacancy per day");
            }

            var application = new Application
            {
                UserId = request.Application.UserId,
                VacancyId = request.Application.VacancyId,
                AppliedDate = DateTime.Now
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

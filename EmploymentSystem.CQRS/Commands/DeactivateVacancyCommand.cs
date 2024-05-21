using EmploymentSystem.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.CQRS.Commands
{
    public class DeactivateVacancyCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeactivateVacancyCommandHandler : IRequestHandler<DeactivateVacancyCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeactivateVacancyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeactivateVacancyCommand request, CancellationToken cancellationToken)
        {
            var vacancy = await _context.Vacancies.FindAsync(request.Id);
            if (vacancy == null)
            {
                throw new Exception("Vacancy not found");
            }

            vacancy.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

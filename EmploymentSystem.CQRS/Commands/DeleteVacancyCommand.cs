using EmploymentSystem.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.CQRS.Commands
{
    public class DeleteVacancyCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteVacancyCommandHandler : IRequestHandler<DeleteVacancyCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteVacancyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteVacancyCommand request, CancellationToken cancellationToken)
        {
            var vacancy = await _context.Vacancies.FindAsync(request.Id);
            if (vacancy == null)
            {
                throw new Exception("Vacancy not found");
            }

            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

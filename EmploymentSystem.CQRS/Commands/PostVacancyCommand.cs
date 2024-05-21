using EmploymentSystem.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.CQRS.Commands
{
    public class PostVacancyCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class PostVacancyCommandHandler : IRequestHandler<PostVacancyCommand>
    {
        private readonly ApplicationDbContext _context;

        public PostVacancyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PostVacancyCommand request, CancellationToken cancellationToken)
        {
            var vacancy = await _context.Vacancies.FindAsync(request.Id);
            if (vacancy == null)
            {
                throw new Exception("Vacancy not found");
            }

            vacancy.IsActive = true;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

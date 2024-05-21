using EmploymentSystem.Data;
using EmploymentSystem.DTOs;
using MediatR;

namespace EmploymentSystem.CQRS.Commands
{
    public class CreateUserCommand : IRequest
    {
        public UserDto User { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateUserCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.User.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.User.Password),
                Role = request.User.Role.ToString()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

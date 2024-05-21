using EmploymentSystem.CQRS.Commands;
using EmploymentSystem.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForVacancy([FromBody] ApplyForVacancyCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetApplicationsByUserId(int userId)
        {
            var applications = await _mediator.Send(new GetApplicationsByUserIdQuery { UserId = userId });
            return Ok(applications);
        }
    }

}

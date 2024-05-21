using EmploymentSystem.CQRS.Commands;
using EmploymentSystem.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacanciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VacanciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetVacancies()
        {
            var vacancies = await _mediator.Send(new GetVacanciesQuery());
            return Ok(vacancies);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVacancy(int id, [FromBody] UpdateVacancyCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            await _mediator.Send(new DeleteVacancyCommand { Id = id });
            return NoContent();
        }

        [HttpPost("{id}/post")]
        public async Task<IActionResult> PostVacancy(int id)
        {
            await _mediator.Send(new PostVacancyCommand { Id = id });
            return NoContent();
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateVacancy(int id)
        {
            await _mediator.Send(new DeactivateVacancyCommand { Id = id });
            return NoContent();
        }

        [HttpGet("{id}/applicants")]
        public async Task<IActionResult> GetApplicantsByVacancyId(int id)
        {
            var applicants = await _mediator.Send(new GetApplicantsByVacancyIdQuery { VacancyId = id });
            return Ok(applicants);
        }
    }


}

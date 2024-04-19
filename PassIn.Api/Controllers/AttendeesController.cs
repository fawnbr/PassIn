using Microsoft.AspNetCore.Mvc;
using PassIn.Application;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api;

[Route("api/[controller]")]
[ApiController]
public class AttendeesController : ControllerBase
{

    [HttpPost]
    [Route("{eventId}/register")]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created) ]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    public IActionResult Register([FromRoute] Guid eventId, [FromBody] RequestRegisterEventJson request)
    {
        var useCase = new RegisterAttendeeOnEventUseCase();
        var response = useCase.Execute(eventId, request);
        return Created(string.Empty, response);
    }

    [HttpGet]
    [Route("{eventId}")]
    [ProducesResponseType(typeof(ResponseAllAttendeesJson), StatusCodes.Status200OK) ]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public IActionResult GetAll([FromRoute] Guid eventId)
    {
        var useCase = new GetAllAttendeesByEventIdUseCase();
        var response = useCase.Execute(eventId);
        return Ok(response);
    }
}

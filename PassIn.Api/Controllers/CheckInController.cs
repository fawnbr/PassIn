using Microsoft.AspNetCore.Mvc;
using PassIn.Application;
using PassIn.Communication.Responses;

namespace PassIn.Api;

[Route("api/[controller]")]
[ApiController]
public class CheckInController : ControllerBase
{
    [HttpPost]
    [Route("{attendeeId}")]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    [Produces("application/json")]
    public IActionResult CheckIn([FromRoute] Guid attendeeId)
    {
        var useCase = new DoAttendeeCheckInUseCase();
        var response = useCase.Execute(attendeeId);
        return Created(string.Empty, response);
    }
}

using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application;

public class DoAttendeeCheckInUseCase
{
    private readonly PassInDbContext _dbContext;
    public DoAttendeeCheckInUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseRegisteredJson Execute(Guid attendeeId)
    {
        Validate(attendeeId);

        var checkIn = new CheckIn
        {
            Attendee_Id = attendeeId,
            Created_at = DateTime.UtcNow
        };

        _dbContext.CheckIns.Add(checkIn);
        _dbContext.SaveChanges();

        return new ResponseRegisteredJson 
        {
            Id = checkIn.Id
        };
    }

    private void Validate(Guid attendeeId)
    {
        var attendeeExists = _dbContext.Attendees.Any(attendee => attendee.Id == attendeeId);
        if (!attendeeExists)
        {
            throw new NotFoundException("The attendee with this id does not exist.");
        }

        var checkInExists = _dbContext.CheckIns.Any(checkIn => checkIn.Attendee_Id == attendeeId);
        if (checkInExists)
        {
            throw new ConflictException("Attendee cannot check in the same event twice.");
        }
    }
}

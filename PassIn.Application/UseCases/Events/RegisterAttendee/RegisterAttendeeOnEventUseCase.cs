using System.Net.Mail;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application;

public class RegisterAttendeeOnEventUseCase
{
    private readonly PassInDbContext _dbContext;

    public RegisterAttendeeOnEventUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseRegisteredJson Execute(Guid eventId, RequestRegisterEventJson request) 
    {   
        Validate(eventId, request);
        var entity = new Infrastructure.Entities.Attendee
        {
            Email = request.Email,
            Name = request.Name,
            Event_Id = eventId,
            Created_At = DateTime.UtcNow,
        };

        _dbContext.Attendees.Add(entity);
        _dbContext.SaveChanges();

        return new ResponseRegisteredJson
        { 
            Id = entity.Id
        };
    }

    private void Validate(Guid eventId, RequestRegisterEventJson request) {
        var eventEntity = _dbContext.Events.Find(eventId);

        if (eventEntity is null) {
            throw new NotFoundException("An event with this id does not exist.");
        }
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException("Name is invalid.");
        }
        if (!EmailIsValid(request.Email))
        {
            throw new ValidationException("Email is invalid.");
        }

        var attendeesAlreadyRegistered = _dbContext.Attendees.Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == eventId);
        if (attendeesAlreadyRegistered) {
            throw new ConflictException("You cannot register twice on the same event.");
        }

        var currentAttendees = _dbContext.Attendees.Count(attendee => attendee.Event_Id == eventId);
        if(currentAttendees == eventEntity.Maximum_Attendees)
        {
            throw new ConflictException("The event has been reached maximum capacity.");
        }
    }

    private bool EmailIsValid(string email)
    {
        try {
            new MailAddress(email);
            return true;
        }
        catch {
            return false;
        }
    }
}

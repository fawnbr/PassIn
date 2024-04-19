using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application;

public class GetAllAttendeesByEventIdUseCase
{
    private readonly PassInDbContext _dbContext;

    public GetAllAttendeesByEventIdUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseAllAttendeesJson Execute(Guid eventId) 
    {
        var eventEntity = _dbContext.Events.Include(ev => ev.Attendees).ThenInclude(attendee => attendee.CheckIn).FirstOrDefault(ev => ev.Id == eventId);
        if (eventEntity is null)
        {
            throw new NotFoundException("An event with id passed doesn't exist.");
        }
        return new ResponseAllAttendeesJson
        {
            Attendees = eventEntity.Attendees.Select(attendee => new ResponseAttendeeJson{
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email,
                CreatedAt = attendee.Created_At,
                CheckedInAt = attendee.CheckIn?.Created_at
            }).ToList()
        };
    }
}

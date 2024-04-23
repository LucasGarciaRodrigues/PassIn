using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure.Repositories;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventId;

public class GetAllAttendeesByEventIdUseCase
{
    private readonly EventsRepository _eventsRepository;

    public GetAllAttendeesByEventIdUseCase()
    {
        _eventsRepository = new EventsRepository();
    }
    
    public ResponseAllAttendeesJson Execute(Guid eventId)
    {
        var entity = _eventsRepository
            .Include(ev => ev.Attendees)
            .ThenInclude(attendee => attendee.CheckIn)
            .FirstOrDefault(ev => ev.Id == eventId);
        
        if (entity is null)
        {
            throw new NotFoundException("Um evento com este id não existe.");
        }
        
        return new ResponseAllAttendeesJson
        {
            Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email,
                CreatedAt = attendee.Created_At,
                CheckedInAt = attendee.CheckIn?.Created_At,
            }).ToList()
        };
    }
}
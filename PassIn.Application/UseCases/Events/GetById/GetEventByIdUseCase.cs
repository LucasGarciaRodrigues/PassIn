using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure.Repositories;

namespace PassIn.Application.UseCases.Events.GetById;

public class GetEventByIdUseCase
{
    private readonly EventsRepository _eventsRepository;

    public GetEventByIdUseCase()
    {
        _eventsRepository = new EventsRepository();
    }
    
    public ResponseEventJson Execute(Guid id)
    {
        var entity = _eventsRepository
            .Include(ev => ev.Attendees)
            .FirstOrDefault(ev => ev.Id == id);
        
        if (entity is null)
        {
            throw new NotFoundException("Um evento com este id não existe.");
        }
        
        return new ResponseEventJson
        {
            Id = entity.Id,
            Title = entity.Title,
            Details = entity.Details,
            MaximumAttendees = entity.Maximum_Attendees,
            AttendeesAmount = entity.Attendees.Count(),
        };
    }
}
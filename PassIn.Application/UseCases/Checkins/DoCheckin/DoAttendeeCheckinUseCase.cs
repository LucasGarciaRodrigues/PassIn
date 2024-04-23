using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure.Repositories;

namespace PassIn.Application.UseCases.Checkins.DoCheckin;

public class DoAttendeeCheckinUseCase
{
    private readonly AttendeesRepository _attendeesRepository;
    private readonly CheckInsRepository _checkInsRepository;

    public DoAttendeeCheckinUseCase()
    {
        _attendeesRepository = new AttendeesRepository();
        _checkInsRepository = new CheckInsRepository();
    }
    
    public ResponseRegisteredJson Execute(Guid attendeeId)
    {
        Validate(attendeeId);

        var entity = new Infrastructure.Entities.CheckIn
        {
            Attendee_Id = attendeeId,
            Created_At = DateTime.UtcNow,
        };
        
        _checkInsRepository.Add(entity);
        
        return new ResponseRegisteredJson
        {
            Id = entity.Id,
        };
    }

    private void Validate(Guid attendeeId)
    {
        var existAttendee = _attendeesRepository.Any(attendee => attendee.Id == attendeeId);
        if (!existAttendee)
        {
            throw new NotFoundException("O participante com este id não foi encontrado.");
        }
        
        var existCheckin = _checkInsRepository.Any(ch => ch.Attendee_Id == attendeeId);
        if (existCheckin)
        {
            throw new ConflictException("O participante não pode fazer o check-in duas vezes no mesmo evento");
        }
    }
}
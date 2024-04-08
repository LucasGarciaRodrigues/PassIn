using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Checkins.DoCheckin;

public class DoAttendeeCheckinUseCase
{
    private readonly PassInDbContext _dbContext;

    public DoAttendeeCheckinUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    
    public ResponseRegisteredJson Execute(Guid attendeeId)
    {
        Validate(attendeeId);

        var entity = new CheckIn
        {
            Attendee_Id = attendeeId,
            Created_At = DateTime.UtcNow,
        };

        _dbContext.CheckIns.Add(entity);
        _dbContext.SaveChanges();
        
        return new ResponseRegisteredJson
        {
            Id = entity.Id,
        };
    }

    private void Validate(Guid attendeeId)
    {
        var existAttendee = _dbContext.Attendees.Any(attendee => attendee.Id == attendeeId);
        if (!existAttendee)
        {
            throw new NotFoundException("O participante com este id não foi encontrado.");
        }

        var existCheckin = _dbContext.CheckIns.Any(ch => ch.Attendee_Id == attendeeId);
        if (existCheckin)
        {
            throw new ConflictException("O participante não pode fazer o check-in duas vezes no mesmo evento");
        }
    }
}
using System.Net.Mail;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.RegisterAttendee;

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
            Name = request.Name,
            Email = request.Email,
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

    private void Validate( Guid eventId, RequestRegisterEventJson request)
    {
        var eventEntity = _dbContext.Events.Find(eventId);
        if (eventEntity is null)
        {
            throw new NotFoundException("Um evento com este id não existe.");
        }
        
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ErrorOnValidationException("O nome é inválido.");
        }
        
        var emailIsValid = EmailIsValid(request.Email);
        if (!emailIsValid)
        {
            throw new ErrorOnValidationException("O e-mail é inválido.");
        }

        var attendeeAlredyRegistered = _dbContext
            .Attendees
            .Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == eventId);

        if (attendeeAlredyRegistered)
        {
            throw new ConflictException("Você não pode se registrar duas vezes no mesmo evento.");
        }

        var attendeesForEvent = _dbContext.Attendees.Count(attendee => attendee.Event_Id == eventId);
        if (attendeesForEvent == eventEntity.Maximum_Attendees)
        {
            throw new ErrorOnValidationException("Não há mais espaço neste evento.");
        }
    }

    private bool EmailIsValid(string email)
    {
        try
        {
            new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
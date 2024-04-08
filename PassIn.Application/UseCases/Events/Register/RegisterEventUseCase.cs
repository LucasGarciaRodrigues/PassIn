using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register;

public class RegisterEventUseCase
{
    public ResponseRegisteredJson Execute(RequestEventJson request)
    {
        Validate(request);
        
        var dbContext = new PassInDbContext();

        var entity = new Infrastructure.Entities.Event
        {
            Title = request.Title,
            Details = request.Details,
            Maximum_Attendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-" ),
        };

        dbContext.Events.Add(entity);
        dbContext.SaveChanges();
       
        return new ResponseRegisteredJson
        {
            Id = entity.Id
        };
    }

    public void Validate(RequestEventJson request)
    {
        if (request.MaximumAttendees < 1)
        {
            throw new ErrorOnValidationException("O número máximo de participantes é inválido.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ErrorOnValidationException("O Título é inválido.");
        }
    }
}
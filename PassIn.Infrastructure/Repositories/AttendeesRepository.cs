using System.Linq.Expressions;
using PassIn.Infrastructure.Entities;

namespace PassIn.Infrastructure.Repositories;

public class AttendeesRepository
{
    private readonly PassInDbContext _dbContext;
    
    public AttendeesRepository()
    {
        _dbContext = new PassInDbContext();
    }

    public void Add(Attendee entity)
    {
        _dbContext.Attendees.Add(entity);
        _dbContext.SaveChanges();
    }

    public bool Any(Expression<Func<Attendee,bool>> predicate)
    {
        return _dbContext.Attendees.Any(predicate);
    }

    public int Count(Expression<Func<Attendee,bool>> predicate)
    {
        return _dbContext.Attendees.Count(predicate);
    }
}
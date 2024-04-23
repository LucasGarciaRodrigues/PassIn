using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PassIn.Infrastructure.Entities;

namespace PassIn.Infrastructure.Repositories;

public class EventsRepository
{
    private readonly PassInDbContext _dbContext;
    
    public EventsRepository()
    {
        _dbContext = new PassInDbContext();
    }

    public void Add(Event entity)
    {
        _dbContext.Events.Add(entity);
    }

    public Event? Find(Guid keyValues)
    {
        return _dbContext.Events.Find(keyValues);
    }
    
    public IIncludableQueryable<Event, List<Attendee>> Include(
        Expression<Func<Event,List<Attendee>>> navigationPropertyPath)
    {
        return _dbContext.Events.Include(navigationPropertyPath);
    }
    
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}
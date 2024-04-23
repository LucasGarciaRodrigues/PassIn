using System.Linq.Expressions;
using PassIn.Infrastructure.Entities;

namespace PassIn.Infrastructure.Repositories;

public class CheckInsRepository
{
    private readonly PassInDbContext _dbContext;
    
    public CheckInsRepository()
    {
        _dbContext = new PassInDbContext();
    }

    public void Add(CheckIn entity)
    {
        _dbContext.CheckIns.Add(entity);
        _dbContext.SaveChanges();
    }

    public bool Any(Expression<Func<CheckIn,bool>> predicate)
    {
        return _dbContext.CheckIns.Any(predicate);
    }
}
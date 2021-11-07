using SocketChat.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace SocketChat.Infrastructure.Persistence.EFCore.Repositories
{
    public abstract class BaseRepository<TType, TContext> where TType : Entity, new() where TContext : EFDataContext
    {
        protected readonly TContext _dbContext;

        protected BaseRepository(TContext context)
        {
            _dbContext = context;
        }

        protected IQueryable<TType> GetEntities()
        {
            return _dbContext.Set<TType>().AsQueryable();
        }

        public virtual async Task<TType> GetAsync(int id)
        {
            return await _dbContext.Set<TType>().AsQueryable()
                .Where(obj => obj.Id == id)
                .FirstOrDefaultAsync();
        }

        public virtual async Task AddAsync(TType obj)
        {
            await _dbContext.Set<TType>().AddAsync(obj);
        }

        public virtual TType Get(int id)
        {
            return _dbContext.Set<TType>().AsQueryable()
                .Where(obj => obj.Id == id)
                .FirstOrDefault();
        }

        public virtual void Add(TType obj)
        {
            _dbContext.Set<TType>().Add(obj);
        }

        public virtual void Update(TType obj)
        {
            _dbContext.Set<TType>().Update(obj);
        }

        public virtual void Remove(TType obj)
        {
            _dbContext.Set<TType>().Remove(obj);
        }
    }
}
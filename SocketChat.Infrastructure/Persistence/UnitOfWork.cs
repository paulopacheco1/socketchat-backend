using SocketChat.Domain.SeedWork;
using SocketChat.Infrastructure.Persistence.EFCore;
using SocketChat.Infrastructure.Persistence.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using SocketChat.Domain.Repositories;

namespace SocketChat.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EFDataContext _context;

        public IUsuariosRepository Usuarios { get; }
        public IConversasRepository Conversas { get; }
        public IMensagensRepository Mensagens { get; }

        public UnitOfWork(EFDataContext context)
        {
            _context = context;

            Usuarios = new EFUsuariosRepository(_context);
            Conversas = new EFConversasRepository(_context);
            Mensagens = new EFMensagensRepository(_context);
        }


        public async Task<int> CommitAsync()
        {
            foreach (var entry in _context.ChangeTracker.Entries<Entity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.SetUpdated();
                        break;
                    case EntityState.Added:
                        entry.Entity.SetCreated();
                        break;
                }
            }

            return await _context.SaveChangesAsync();
        }
    }
}
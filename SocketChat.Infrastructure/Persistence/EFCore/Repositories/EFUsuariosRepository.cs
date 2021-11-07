using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocketChat.Domain.Aggregates;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketChat.Infrastructure.Persistence.EFCore.Repositories
{
    public class EFUsuariosRepository : BaseRepository<Usuario, EFDataContext>, IUsuariosRepository
    {
        public EFUsuariosRepository(EFDataContext context) : base(context) { }

        public async Task<List<Usuario>> ListAsync(UsuarioFilter filter)
        {
            return await GetEntities()
                .Where(u => (string.IsNullOrEmpty(filter.Email) || u.Email == filter.Email))
                .Where(u => (string.IsNullOrEmpty(filter.Nome) || u.Nome.ToLower().StartsWith(filter.Nome.ToLower())))
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public override async Task<Usuario> GetAsync(int id)
        {
            return await GetEntities()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await GetEntities()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }
    }

    public class UsuarioConfigurations : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property("Senha");
        }
    }
}
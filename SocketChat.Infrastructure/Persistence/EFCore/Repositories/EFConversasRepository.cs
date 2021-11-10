using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocketChat.Domain.Entities;
using SocketChat.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketChat.Infrastructure.Persistence.EFCore.Repositories
{
    public class EFConversasRepository : BaseRepository<Conversa, EFDataContext>, IConversasRepository
    {
        public EFConversasRepository(EFDataContext context) : base(context) { }

        public async Task<List<Conversa>> ListAsync(ConversaFilter filter)
        {
            return await GetEntities()
                .Where(c => (string.IsNullOrEmpty(filter.BuscaNomeOuParticipante) || c.Nome.ToLower().StartsWith(filter.BuscaNomeOuParticipante.ToLower()) || c.Participantes.Any(p => p.Nome.ToLower().StartsWith(filter.BuscaNomeOuParticipante.ToLower()))))
                .Where(c => (filter.Participante == null || c.Participantes.Any(p => p.Nome.ToLower().StartsWith(filter.Participante.Nome.ToLower()))))
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }
    }

    public class ConversaConfigurations : IEntityTypeConfiguration<Conversa>
    {
        public void Configure(EntityTypeBuilder<Conversa> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
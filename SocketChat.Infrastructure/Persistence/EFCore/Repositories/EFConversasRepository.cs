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

        public async Task<Conversa> GetAsync(List<int> idParticipantes)
        {
            return await GetEntities()
                .Where(c => (c.Participantes.Count == idParticipantes.Count && c.Participantes.All(p => idParticipantes.Any(id => p.Id == id))))
                .Include(c => c.Participantes)
                .FirstOrDefaultAsync();
        }

        public override async Task<Conversa> GetAsync(int idParticipante)
        {
            return await GetEntities()
                .Where(c => c.Id == idParticipante)
                .Include(c => c.Participantes)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Conversa>> ListAsync(int idParticipante, ConversaFilter filter)
        {
            return await GetEntities()
                .Where(c => (c.Participantes.Any(p => p.Id == idParticipante)))
                .Where(c => (string.IsNullOrEmpty(filter.BuscaNomeOuParticipante) || c.Nome.ToLower().StartsWith(filter.BuscaNomeOuParticipante.ToLower()) || c.Participantes.Where(p => p.Id != idParticipante).Any(p => p.Nome.ToLower().StartsWith(filter.BuscaNomeOuParticipante.ToLower()))))
                .Include(c => c.Mensagens.OrderByDescending(m => m.DataEnvio).Take(1))
                .Include(c => c.Participantes)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }
    }

    public class ConversaConfigurations : IEntityTypeConfiguration<Conversa>
    {
        public void Configure(EntityTypeBuilder<Conversa> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasMany(c => c.Mensagens)
                .WithOne()
                .HasForeignKey(m => m.IdConversa)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Participantes)
                .WithMany(p => p.Conversas);
        }
    }
}
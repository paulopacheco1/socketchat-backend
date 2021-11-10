using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocketChat.Domain.Entities;
using SocketChat.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketChat.Infrastructure.Persistence.EFCore.Repositories
{
    public class EFMensagensRepository : BaseRepository<Mensagem, EFDataContext>, IMensagensRepository
    {
        public EFMensagensRepository(EFDataContext context) : base(context) { }

        public async Task<List<Mensagem>> ListAsync(MensagemFilter filter)
        {
            return await GetEntities()
                .Where(m => (filter.Remetente == null || m.Remetente.Id == filter.Remetente.Id))
                .Where(m => (string.IsNullOrEmpty(filter.Conteudo) || m.Conteudo.ToLower().StartsWith(filter.Conteudo.ToLower())))
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .OrderBy(m => m.DataEnvio)
                .ToListAsync();
        }
    }

    public class MensagemConfigurations : IEntityTypeConfiguration<Mensagem>
    {
        public void Configure(EntityTypeBuilder<Mensagem> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
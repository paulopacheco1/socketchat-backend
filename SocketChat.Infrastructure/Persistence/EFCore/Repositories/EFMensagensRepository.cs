using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocketChat.Domain.Entities;
using SocketChat.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketChat.Infrastructure.Persistence.EFCore.Repositories
{
    public class EFMensagensRepository : BaseRepository<Mensagem, EFDataContext>, IMensagensRepository
    {
        public EFMensagensRepository(EFDataContext context) : base(context) { }

        public async Task<List<Mensagem>> ListAsync(int idConversa, MensagemFilter filter)
        {
            return await GetEntities()
                .Where(m => (m.IdConversa == idConversa))
                .Where(m => (filter.BeforeDate == default || m.DataEnvio < filter.BeforeDate))
                .Where(m => (string.IsNullOrEmpty(filter.Conteudo) || m.Conteudo.ToLower().StartsWith(filter.Conteudo.ToLower())))
                .OrderByDescending(m => m.DataEnvio)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
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
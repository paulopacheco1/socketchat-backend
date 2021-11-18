using SocketChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SocketChat.Infrastructure.Persistence.EFCore
{
    public class EFDataContext : DbContext
    {
        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Conversa> Conversas { get; set; }
        DbSet<Mensagem> Mensagens { get; set; }

        public EFDataContext(DbContextOptions<EFDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
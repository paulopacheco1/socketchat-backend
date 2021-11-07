using SocketChat.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SocketChat.Infrastructure.Persistence.EFCore
{
    public class EFDataContext : DbContext
    {
        DbSet<Usuario> Usuarios { get; set; }

        public EFDataContext(DbContextOptions<EFDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
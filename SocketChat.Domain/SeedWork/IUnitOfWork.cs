using SocketChat.Domain.Aggregates;
using System.Threading.Tasks;

namespace SocketChat.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();

        IUsuariosRepository Usuarios { get; }
    }
}
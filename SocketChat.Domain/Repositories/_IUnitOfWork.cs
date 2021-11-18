using System.Threading.Tasks;

namespace SocketChat.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();

        IUsuariosRepository Usuarios { get; }
        IConversasRepository Conversas { get; }
        IMensagensRepository Mensagens { get; }
    }
}
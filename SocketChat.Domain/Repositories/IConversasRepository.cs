using SocketChat.Domain.Entities;
using SocketChat.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketChat.Domain.Repositories
{
    public interface IConversasRepository
    {
        Task AddAsync(Conversa conversa);
        void Update(Conversa conversa);
        void Remove(Conversa conversa);
        Task<List<Conversa>> ListAsync(ConversaFilter filter);
        Task<Conversa> GetAsync(int id);
    }

    public class ConversaFilter : Filter
    {
        public string BuscaNomeOuParticipante { get; set; }
        public Usuario Participante { get; set; }
    }
}

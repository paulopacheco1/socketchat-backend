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
        Task<List<Conversa>> ListAsync(int idParticipante, ConversaFilter filter);
        Task<Conversa> GetAsync(int id);
        Task<Conversa> GetAsync(List<int> idParticipantes);
    }

    public class ConversaFilter : Filter
    {
        public string BuscaNomeOuParticipante { get; set; }
    }
}

using SocketChat.Domain.Entities;
using SocketChat.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketChat.Domain.Repositories
{
    public interface IMensagensRepository
    {
        Task AddAsync(Mensagem mensagem);
        void Update(Mensagem mensagem);
        void Remove(Mensagem mensagem);
        Task<List<Mensagem>> ListAsync(MensagemFilter filter);
        Task<Mensagem> GetAsync(int id);
    }

    public class MensagemFilter : Filter
    {
        public string Conteudo { get; set; }
        public Usuario Remetente { get; set; }
    }
}

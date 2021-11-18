using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocketChat.Domain.SeedWork;

namespace SocketChat.Domain.Entities
{
    public class Mensagem : Entity
    {
        public string Conteudo { get; private set; }
        public DateTime DataEnvio { get; private set; }
        public int IdConversa { get; private set; }
        public int IdRemetente { get; private set; }

        public Mensagem() { }

        public static Mensagem Create(Conversa conversa, Usuario remetente, string conteudo)
        {
            return new Mensagem()
            {
                IdConversa = conversa.Id,
                IdRemetente = remetente.Id,
                Conteudo = conteudo,
                DataEnvio = DateTime.Now,
            };
        }
    }
}
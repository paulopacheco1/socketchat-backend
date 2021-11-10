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
        public Usuario Remetente { get; private set; }

        public Mensagem() { }

        public static Mensagem Create(Usuario remetente, string conteudo)
        {
            return new Mensagem()
            {
                Remetente = remetente,
                Conteudo = conteudo,
                DataEnvio = DateTime.Now,
            };
        }
    }
}
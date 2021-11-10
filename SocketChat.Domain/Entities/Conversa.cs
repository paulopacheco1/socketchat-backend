using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocketChat.Domain.SeedWork;

namespace SocketChat.Domain.Entities
{
    public class Conversa : Entity
    {
        public string Nome { get; private set; }
        public List<Usuario> Participantes { get; private set; }
        public List<Mensagem> Mensagens { get; private set; }

        public Conversa() { }

        public static Conversa Create(List<Usuario> participantes, string nome = "")
        {
            return new Conversa()
            {
                Nome = nome,
                Participantes = participantes,
            };
        }
    }
}
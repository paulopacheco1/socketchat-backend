using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocketChat.Domain.Exceptions;
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

        public string GetDisplayName(Usuario participante)
        {
            var participanteEstaNaConversa = Participantes.Any(p => p.Id == participante.Id);
            if (!participanteEstaNaConversa) throw new AppException("Usuário não participa da conversa");

            if (!String.IsNullOrWhiteSpace(Nome)) return Nome;

            return string.Join(", ", Participantes.Where(p => p.Id != participante.Id).Select(p => p.Nome));
        }
    }
}
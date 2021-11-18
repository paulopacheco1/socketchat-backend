using System;
using System.Collections.Generic;
using SocketChat.Domain.Entities;

namespace SocketChat.Application.ViewModels
{
    public class ConversaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<ConversaParticipanteViewModel> Participantes { get; set; }
        public List<Mensagem> Mensagens { get; set; }
    }

    public class ConversaParticipanteViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
using SocketChat.Domain.Entities;
using SocketChat.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketChat.Domain.Repositories
{
    public interface IUsuariosRepository
    {
        Task AddAsync(Usuario usuario);
        void Update(Usuario usuario);
        void Remove(Usuario usuario);
        Task<List<Usuario>> ListAsync(UsuarioFilter filter);
        Task<Usuario> GetAsync(int id);
        Task<Usuario> GetByEmailAsync(string email);
    }

    public class UsuarioFilter : Filter
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}

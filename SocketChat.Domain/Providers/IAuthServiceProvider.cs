using System.Security.Claims;
using SocketChat.Domain.Entities;

namespace SocketChat.Domain.Providers
{
    public interface IAuthServiceProvider
    {
        string CriarTokenJwt(Usuario usuario);
        ClaimsPrincipal ValidarTokenJwt(string token);
    }

    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public string AccessSecret { get; set; }
        public long AccessValidFor { get; set; }
    }
}
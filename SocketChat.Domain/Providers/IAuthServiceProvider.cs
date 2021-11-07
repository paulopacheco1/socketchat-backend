using SocketChat.Domain.Aggregates;

namespace SocketChat.Domain.Providers
{
    public interface IAuthServiceProvider
    {
        string CriarTokenJwt(Usuario usuario);
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
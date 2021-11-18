using SocketChat.Domain.Providers;
using System;

namespace SocketChat.API.AccessContexts
{
    public class WsAccessContextProvider : IAccessContextProvider
    {
        private readonly IAuthServiceProvider _authService;
        private readonly string _token;
        public WsAccessContextProvider(IAuthServiceProvider authService, string token)
        {
            _authService = authService;
            _token = token;
        }

        public AccessContext Get()
        {
            if (String.IsNullOrEmpty(_token)) return null;

            var claims = _authService.ValidarTokenJwt(_token);

            var currentUserId = claims.GetUserId();
            var currentUserEmail = claims.GetUserEmail();

            return new AccessContext(currentUserId, currentUserEmail);
        }
    }
}

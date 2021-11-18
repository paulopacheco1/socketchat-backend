using Microsoft.AspNetCore.Http;
using SocketChat.Domain.Providers;

namespace SocketChat.API.AccessContexts
{
    public class HttpAccessContextProvider : IAccessContextProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpAccessContextProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Domain.Providers.AccessContext Get()
        {
            if (_accessor == null) return null;
            if (_accessor.HttpContext == null) return null;
            if (_accessor.HttpContext.User == null) return null;
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated) return null;

            var currentUserId = _accessor.HttpContext.User.GetUserId();
            var currentUserEmail = _accessor.HttpContext.User.GetUserEmail();

            return new Domain.Providers.AccessContext(currentUserId, currentUserEmail);
        }
    }
}

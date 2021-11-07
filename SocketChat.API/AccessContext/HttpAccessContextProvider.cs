using Microsoft.AspNetCore.Http;
using SocketChat.Domain.Providers;
using System;
using System.Security.Claims;

namespace SocketChat.API.HttpAccessContext
{
    public class HttpAccessContextProvider : IAccessContextProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpAccessContextProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public AccessContext Get()
        {
            if (_accessor == null) return null;
            if (_accessor.HttpContext == null) return null;
            if (_accessor.HttpContext.User == null) return null;
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated) return null;

            var currentUserId = _accessor.HttpContext.User.GetUserId();
            var currentUserEmail = _accessor.HttpContext.User.GetUserEmail();

            return new AccessContext(currentUserId, currentUserEmail);
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var stringId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _ = int.TryParse(stringId, out var currentUserId);

            return currentUserId;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}

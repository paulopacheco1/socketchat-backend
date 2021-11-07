using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocketChat.Domain.Providers;

namespace SocketChat.API.Controllers
{
    [Authorize]
    public class HttpControllerBase : ControllerBase
    {
        protected readonly IAccessContextProvider _accessContextProvider;
        protected readonly IMediator _mediator;

        public HttpControllerBase(IAccessContextProvider accessContextProvider, IMediator mediator)
        {
            _accessContextProvider = accessContextProvider;
            _mediator = mediator;
        }

        protected int Id()
        {
            return _accessContextProvider.Get().UserId;
        }
    }
}

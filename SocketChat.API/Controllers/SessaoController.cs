using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocketChat.Application.Commands;
using SocketChat.Domain.Providers;
using System.Threading.Tasks;

namespace SocketChat.API.Controllers
{
    [Route("api/sessao")]
    public class SessaoController : HttpControllerBase
    {
        public SessaoController(IAccessContextProvider accessContextProvider, IMediator mediator) : base(accessContextProvider, mediator)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateSessaoCommand command)
        {
            var sessaoViewModel = await _mediator.Send(command);
            return Ok(sessaoViewModel);
        }
    }
}
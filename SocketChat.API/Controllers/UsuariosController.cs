using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocketChat.API.AccessContexts;
using SocketChat.Application.Commands;
using SocketChat.Application.Queries;
using SocketChat.Domain.Providers;
using SocketChat.Domain.Repositories;
using System.Threading.Tasks;

namespace SocketChat.API.Controllers
{
    [Route("api/usuarios")]
    public class UsuariosController : HttpControllerBase
    {
        public UsuariosController(IAccessContextProvider accessContextProvider, IMediator mediator) : base(accessContextProvider, mediator)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, command);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UsuarioFilter filter)
        {
            var query = new ListUsuariosQuery(filter);
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUsuarioByIdQuery(id);
            var user = await _mediator.Send(query);
            return Ok(user);
        }
    }

    [Route("api/usuarios/me")]
    public class MeController : HttpControllerBase
    {
        public MeController(IAccessContextProvider accessContextProvider, IMediator mediator) : base(accessContextProvider, mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetUsuarioByIdQuery(Id());
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUsuarioCommand command)
        {
            command.Id = Id();
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
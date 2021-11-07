using SocketChat.Application.Exceptions;
using SocketChat.Domain.Providers;
using SocketChat.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Commands
{
    public class CreateSessaoCommand : Command<SessaoViewModel>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class CreateSessaoCommandHandler : CommandHandler<CreateSessaoCommand, SessaoViewModel>
    {
        private readonly IAuthServiceProvider _authService;
        public CreateSessaoCommandHandler(IUnitOfWork unitOfWork, IAuthServiceProvider authService) : base(unitOfWork)
        {
            _authService = authService;
        }

        public async override Task<SessaoViewModel> Handle(CreateSessaoCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Usuarios.GetByEmailAsync(request.Email);

            if (user == null || !user.CheckPassword(request.Senha)) throw new FalhaNoLoginException();

            var token = _authService.CriarTokenJwt(user);
            return new SessaoViewModel(user.Email, token);
        }
    }
}
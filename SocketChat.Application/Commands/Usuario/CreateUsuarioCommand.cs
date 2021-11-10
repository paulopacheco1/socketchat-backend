using SocketChat.Application.Exceptions;
using SocketChat.Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using SocketChat.Domain.Repositories;

namespace SocketChat.Application.Commands
{
    public class CreateUsuarioCommand : Command<int>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class CreateUsuarioCommandHandler : CommandHandler<CreateUsuarioCommand, int>
    {
        public CreateUsuarioCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<int> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
        {
            var emailJaCadastrado = await _unitOfWork.Usuarios.GetByEmailAsync(request.Email) != null;
            if (emailJaCadastrado) throw new BadRequestException("E-mail j� cadastrado no sistema.");

            var usuario = Usuario.Create(new CreateUsuarioDTO
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha,
            });

            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.CommitAsync();
            return usuario.Id;
        }
    }

    public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
    {
        public CreateUsuarioCommandValidator()
        {
            RuleFor(p => p.Nome)
                .NotNull()
                .WithMessage("Campo obrigat�rio")
                .NotEmpty()
                .WithMessage("Campo obrigat�rio");

            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Campo obrigat�rio")
                .NotEmpty()
                .WithMessage("Campo obrigat�rio")
                .EmailAddress()
                .WithMessage("Email inv�lido");

            RuleFor(p => p.Senha)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Campo obrigat�rio")
                .NotEmpty()
                .WithMessage("Campo obrigat�rio")
                .MinimumLength(6)
                .WithMessage("A senha deve conter ao menos 6 caracteres");
        }
    }
}
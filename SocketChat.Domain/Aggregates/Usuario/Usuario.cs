using SocketChat.Domain.Exceptions;
using SocketChat.Domain.Helpers;
using SocketChat.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocketChat.Domain.Aggregates
{
    public class Usuario : Entity
    {
        public string Nome { get; set; }
        public string Email { get; private set; }

        private string Senha;

        public Usuario() { }

        public static Usuario Create(CreateUsuarioDTO dto)
        {
            if (!IsSenhaValida(dto.Senha)) throw new AppException("A senha deve conter ao menos 8 caracteres, contendo pelo menos uma letra maiúscula, uma minúscula, um número e um caractere especial.");

            return new Usuario()
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = PasswordHasher.ComputeSHA256Hash(dto.Senha),
            };
        }

        public static bool IsSenhaValida(string password)
        {
            return password.Length >= 6;
            //var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");
            //return regex.IsMatch(password);
        }

        public bool CheckPassword(string password) => PasswordHasher.ComputeSHA256Hash(password) == Senha;

    }

    public class CreateUsuarioDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}

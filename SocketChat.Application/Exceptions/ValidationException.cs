using SocketChat.Domain.Exceptions;
using System.Collections.Generic;

namespace SocketChat.Application.Exceptions
{
    public class ValidationException : AppException
    {
        public IEnumerable<ValidationFieldErrors> Fields { get; }

        public ValidationException(IEnumerable<ValidationFieldErrors> fields) : base("Erro de validação. Verifique seus dados.")
        {
            Fields = fields;
        }
    }

    public class ValidationFieldErrors
    {
        public string Field { get; }
        public IEnumerable<string> Errors { get; }

        public ValidationFieldErrors(string field, IEnumerable<string> errors)
        {
            Field = field;
            Errors = errors;
        }
    }
}

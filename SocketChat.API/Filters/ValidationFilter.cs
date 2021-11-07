using SocketChat.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace SocketChat.API.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var fieldErrors = context.ModelState
                    .Select(ms => new ValidationFieldErrors(ms.Key, ms.Value.Errors.Select(e => e.ErrorMessage)));

                throw new ValidationException(fieldErrors);
            }
        }
    }
}
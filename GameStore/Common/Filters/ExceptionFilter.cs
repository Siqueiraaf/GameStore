using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.PackingService.Common.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var mensagem = "Ocorreu um erro inesperado. Por favor, tente novamente.";
        object? detalhes = null;

        // Exemplo de tratamento para exceções específicas
        if (context.Exception is ArgumentException argEx)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            mensagem = argEx.Message;
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
            mensagem = "Acesso não autorizado.";
        }
        else
        {
            // Em ambiente de desenvolvimento, exibe detalhes da exceção
            var env = context.HttpContext.RequestServices.GetService(
                typeof(Microsoft.Extensions.Hosting.IHostEnvironment))
                as Microsoft.Extensions.Hosting.IHostEnvironment;
            if (env != null && env.IsDevelopment())
            {
                detalhes = context.Exception.ToString();
            }
        }

        context.HttpContext.Response.StatusCode = statusCode;
        context.Result = new JsonResult(new
        {
            Mensagem = mensagem,
            Detalhes = detalhes
        });
        context.ExceptionHandled = true;
    }
}

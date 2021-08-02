using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuggestMusic.Infrastructure.Exceptions.HttpExceptions;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SuggestMusic.Infrastructure.Exceptions
{
    public static class ConfigureExceptionHandling
    {
        /// <summary>
        /// Configura exceção customizada para a solução, trata exceções globais, validações, e gravação de log.
        /// </summary>
        /// <param name="app">Instância da aplicação</param>
        /// <param name="loggerFactory">Instância da interface de log</param>
        public static async Task<Task> Configure(HttpContext context, ILoggerFactory loggerFactory)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

            if (exceptionObject != null)
            {
                var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");

                if (exceptionObject.Error is CustomHttpException exception)
                {
                    context.Response.StatusCode = exception.StatusCode;
                    logger.LogWarning($"Unexpected http status: {exceptionObject.Error}");

                    await context.Response.WriteAsync(exception.GetResponse()).ConfigureAwait(false);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    logger.LogError($"Unexpected error: {exceptionObject.Error}");
                    var errorMessage = GetTemplateError(exceptionObject, context);

                    await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Obtém o template padrão de erro a ser retornado
        /// </summary>
        /// <param name="exceptionObject">Instância com as informações da exceção gerada</param>
        /// <param name="context">Contexto HTTP</param>
        /// <returns>Objeto serializado</returns>
        private static string GetTemplateError(IExceptionHandlerFeature exceptionObject, HttpContext context)
        {
            return JsonConvert.SerializeObject(new
            {
                context.Response.StatusCode,
                Detailed = "An error occurred whilst processing your request",
                exceptionObject.Error.Message,
                exceptionObject.Error.StackTrace
            });
        }
    }
}

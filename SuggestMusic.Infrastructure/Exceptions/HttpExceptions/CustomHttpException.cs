using Newtonsoft.Json;
using System;

namespace SuggestMusic.Infrastructure.Exceptions.HttpExceptions
{
    /// <summary>
    /// Classe abstrata que define o modelo para exceções customizadas HTTP
    /// </summary>
    public abstract class CustomHttpException : Exception
    {
        public CustomHttpException(string message) : base(message) { }

        /// <summary>
        /// Define o código do status a ser retornado para o tipo de exceção, definido pelas subclasses
        /// </summary>
        public abstract int StatusCode { get; }

        /// <summary>
        /// Define o response a ser retornado para o tipo de exceção
        /// </summary>
        public string GetResponse()
        {
            return JsonConvert.SerializeObject(new
            {
                StatusCode,
                Message
            });
        }
    }
}

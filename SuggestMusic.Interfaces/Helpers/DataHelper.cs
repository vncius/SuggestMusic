using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestMusic.Interfaces.Helpers
{
    /// <summary>
    /// Classe de apoio com metodos de extensão para tratamento de dados
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Converte string para a codificação base64
        /// </summary>
        /// <param name="value">Valor a ser codificado em base64</param>
        /// <returns>O valor codificado em base64</returns>
        public static string ConvertToBase64(this string value)
        {
            byte[] texto = Encoding.ASCII.GetBytes(value);
            return Convert.ToBase64String(texto);
        }

        /// <summary>
        /// Adiciona query string a uma URL
        /// </summary>
        /// <param name="url">URL Destino</param>
        /// <param name="parameters">Parâmetros a ser incluido na URL</param>
        /// <returns>URL com query string adicionada</returns>
        public static string AddQueryString(this string url, Dictionary<string, string> parameters)
        {
            return QueryHelpers.AddQueryString(url, parameters);
        }

        /// <summary>
        /// Adiciona relative path a uma URL
        /// </summary>
        /// <param name="url">URL Destino</param>
        /// <param name="relativePath">Relative path a ser concatenado na URL Destino</param>
        /// <returns>URL concatenada com relative path</returns>
        public static string CombineUrl(this string url, string relativePath)
        {
            if (url.EndsWith('/'))
            {
                url = url.Remove(url.Length - 1, 1);
            }

            if (relativePath.StartsWith('/'))
            {
                relativePath = relativePath.Remove(0, 1);
            }

            return $"{url}/{relativePath}";
        }
    }
}

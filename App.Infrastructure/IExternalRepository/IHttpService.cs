using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.IExternalRepository
{
    public interface IHttpService
    {
        Task<T?> GetAsync<T>( string url,Dictionary<string, string>? headers = null);

        Task<TResponse?> PostAsync<TRequest, TResponse>(string url,TRequest request,
            Dictionary<string, string>? headers = null);

        Task<TResponse?> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string>? headers = null);

        Task DeleteAsync(
            string url,
            Dictionary<string, string>? headers = null);
    }
}

using App.Application.IExternalRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Text.Json;

namespace App.Infrastructure.ExternalServices
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(
            string url,
            Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                url);

            AddHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string>? headers = null)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                url);

            AddHeaders(httpRequest, headers);

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(content);
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string>? headers = null)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Put,
                url);

            AddHeaders(httpRequest, headers);

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(content);
        }

        public async Task DeleteAsync(
            string url,
            Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                url);

            AddHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        private static void AddHeaders(
            HttpRequestMessage request,
            Dictionary<string, string>? headers)
        {
            if (headers == null) return;

            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }
        }
    }
}

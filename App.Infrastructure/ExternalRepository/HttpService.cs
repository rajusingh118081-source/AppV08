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

        public async Task<T?> GetAsync<T>(string url,Dictionary<string, string>? headers = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get,url);

            AddHeaders(request, headers);

            using var response = await _httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HandleHttpException(
                    response.StatusCode,
                    content);
            }

            if (string.IsNullOrWhiteSpace(content))
                return default;

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
            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                url);

            AddHeaders(httpRequest, headers);

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            using var response =
                await _httpClient.SendAsync(httpRequest);

            var content =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HandleHttpException(
                    response.StatusCode,
                    content);
            }

            if (string.IsNullOrWhiteSpace(content))
                return default;

            return JsonSerializer.Deserialize<TResponse>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        // For QuickBooks OAuth token API
        public async Task<TResponse?> PostFormAsync<TResponse>(
            string url,
            Dictionary<string, string> formData,
            Dictionary<string, string>? headers = null)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                url);

            AddHeaders(request, headers);

            request.Content =
                new FormUrlEncodedContent(formData);

            using var response =
                await _httpClient.SendAsync(request);

            var content =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HandleHttpException(
                    response.StatusCode,
                    content);
            }

            if (string.IsNullOrWhiteSpace(content))
                return default;

            return JsonSerializer.Deserialize<TResponse>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string>? headers = null)
        {
            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Put,
                url);

            AddHeaders(httpRequest, headers);

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            using var response =
                await _httpClient.SendAsync(httpRequest);

            var content =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HandleHttpException(
                    response.StatusCode,
                    content);
            }

            if (string.IsNullOrWhiteSpace(content))
                return default;

            return JsonSerializer.Deserialize<TResponse>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task DeleteAsync(
            string url,
            Dictionary<string, string>? headers = null)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Delete,
                url);

            AddHeaders(request, headers);

            using var response =
                await _httpClient.SendAsync(request);

            var content =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HandleHttpException(
                    response.StatusCode,
                    content);
            }
        }

        private static void AddHeaders(
            HttpRequestMessage request,
            Dictionary<string, string>? headers)
        {
            if (headers == null)
                return;

            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }
        }
    }
}

using App.Application.DTOs.Main_DTO;
using App.Application.IExternalRepository;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Common.GenericResponse;
using App.Domain.Entities;
using App.Domain.Entities.QuickBooksOnline;
using App.Infrastructure.ExternalServices;
using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Infrastructure.ExternalRepository.QBO
{
    public class QBOService : IQuickBooksOnline
    {
        private readonly OAuth2Client _oauthClient;
        private readonly ILogger<QBOService> _logger;
        private readonly IHttpService _httpClient;
        private readonly QBOSettings _qboSettings;
        private readonly IQuickBooksTokenRep _tokenRep;
        public QBOService(IConfiguration config, ILogger<QBOService> logger,IOptions<QBOSettings> options, IHttpService httpService
            ,IQuickBooksTokenRep tokenRep)
        {
            _logger = logger;
            _httpClient = httpService;
            _qboSettings = options.Value;

            var settings = _qboSettings;

            _oauthClient = new OAuth2Client(
                settings.ClientId,
                settings.ClientSecret,
                settings.RedirectUri,
                settings.Environment);
            _logger.LogInformation("Customer Sync Started. CorrelationId:{CorrelationId}", _oauthClient.ClientID);
            _tokenRep= tokenRep;
        }

        public string GetAuthorizationUrl()
        {
            return _oauthClient.GetAuthorizationURL(new List<OidcScopes>
            {
             OidcScopes.Accounting
            });
        }

        public async Task<TokenResponse> GetBearerTokenAsync(string code, string realmId)
        {
            var tokenResponse = await _oauthClient.GetBearerTokenAsync(code);
            return tokenResponse;
        }

        public async Task<Response> RefreshQuickBooksTokenAsync(QuickBooksToken token)
        {
            Response _response = new Response();
            var clientId = _qboSettings.ClientId;

            var clientSecret = _qboSettings.ClientSecret;

            if (string.IsNullOrWhiteSpace(clientId))
                throw new Exception("QuickBooks ClientId is missing.");

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new Exception("QuickBooks ClientSecret is missing.");

            if (string.IsNullOrWhiteSpace(token.RefreshToken))
                throw new Exception("QuickBooks RefreshToken is missing.");

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            var headers = new Dictionary<string, string>
            {
                ["Authorization"] = $"Basic {credentials}",
                ["Accept"] = "application/json"
            };

            var formData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = token.RefreshToken
            };

            var response = await _httpClient.PostFormAsync<QuickBooksTokenResponse>
                ("https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer",
                    formData,
                    headers);

            if (response == null || string.IsNullOrWhiteSpace(response.AccessToken))
            {
                throw new Exception("QuickBooks token refresh failed. Access token is empty.");
            }

            // Update access token
            token.AccessToken = response.AccessToken;

            // QuickBooks may return a new refresh token.
            // Always save it when provided.
            if (!string.IsNullOrWhiteSpace(response.RefreshToken))
            {
                token.RefreshToken = response.RefreshToken;
            }
            // Optional if your entity contains these fields
            token.AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn);

            token.RefreshTokenExpiresAt =DateTime.UtcNow.AddSeconds(response.XRefreshTokenExpiresIn);

            // Save updated token
            await _tokenRep.UpdateTokenAsync(token);
            return _response;
        }

    }

    public class QuickBooksTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("x_refresh_token_expires_in")]
        public int XRefreshTokenExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
    }
}

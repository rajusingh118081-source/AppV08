using App.Application.DTOs.Main_DTO;
using App.Common.GenericResponse;
using App.Domain.Entities.QuickBooksOnline;
using Intuit.Ipp.OAuth2PlatformClient;
using System;
using System.Collections.Generic;
using System.Text;
namespace App.Application.IExternalRepository.QuickBookOnline
{
    public interface IQuickBooksOnline
    {
        string GetAuthorizationUrl();

        Task<TokenResponse> GetBearerTokenAsync(string authorizationCode, string realmId);
        Task<Response> RefreshQuickBooksTokenAsync(QuickBooksToken token);
    }
}

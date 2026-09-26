using App.Application.IExternalRepository.QuickBookOnline;
using App.Common.GenericResponse;
using App.Domain.Entities.QuickBooksOnline;

namespace App.Application.DataSyncLayer
{
    public class SyncDataQuickBooksToken
    {
        private readonly IQuickBooksOnline _booksOnline;
        private readonly IQuickBooksTokenRep _booksToken;
        private readonly IUnitOfWork _unitOfWork;

        public SyncDataQuickBooksToken(IQuickBooksOnline booksOnline,IQuickBooksTokenRep booksToken,IUnitOfWork unitOfWork)
        {
            _booksOnline = booksOnline?? throw new ArgumentNullException(nameof(booksOnline));
            _booksToken = booksToken?? throw new ArgumentNullException(nameof(booksToken));
            _unitOfWork = unitOfWork?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Response> GetBearerTokenAsync(string authorizationCode,string realmId)
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                return new Response
                {
                    Status = false,
                    Message = "Authorization code is required."
                };
            }

            if (string.IsNullOrWhiteSpace(realmId))
            {
                return new Response
                {
                    Status = false,
                    Message = "RealmId is required."
                };
            }

            var tokenResponse = await _booksOnline.GetBearerTokenAsync(authorizationCode,realmId);

            if (tokenResponse == null)
            {
                return new Response
                {
                    Status = false,
                    Message ="QuickBooks token response is null."
                };
            }
            var now = DateTime.UtcNow;
            var token = new QuickBooksToken
            {
                AccessToken =tokenResponse.AccessToken,
                RefreshToken =tokenResponse.RefreshToken,
                TokenType =tokenResponse.TokenType,
                RealmId =realmId,
                UniqueNumber =Guid.NewGuid().ToString(),
                AccessTokenExpiresAt =now.AddSeconds(tokenResponse.AccessTokenExpiresIn),
                RefreshTokenExpiresAt =now.AddSeconds(tokenResponse.RefreshTokenExpiresIn)
            };
            var result =await _booksToken.CreateAsync(token);
            if (!result.Status)
            {
                return result;
            }
            await _unitOfWork.SaveChangesAsync();
            return new Response
            {
                Status = true,
                Message =
                    "QuickBooks connected successfully.",
                ReturnResponse = tokenResponse
            };
        }
    }
}

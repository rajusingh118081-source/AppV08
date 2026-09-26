using AapRepository;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Common.GenericResponse;
using App.Domain.Entities.QuickBooksOnline;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.ExternalRepository.QuickBooksOnline
{
    public class QuickBooksTokenRep: Repository<QuickBooksToken>, IQuickBooksTokenRep
    {
        public QuickBooksTokenRep(DB_Contexts context): base(context)
        {
        }

        public async Task<Response> CreateAsync(QuickBooksToken quickBooksToken)
        {
            await AddAsync(quickBooksToken);
            return new Response
            {
                Status = true,
                Message ="QuickBooks token added."
            };
        }

        public async Task<Response> UpdateTokenAsync(QuickBooksToken quickBooksToken)
        {
            if (quickBooksToken == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "QuickBooks token is required."
                };
            }

            await UpdateAsync(quickBooksToken);
            return new Response
            {
                Status = true,
                Message = "QuickBooks token updated successfully.",
                ReturnResponse = quickBooksToken
            };
        }

        public async Task<QuickBooksToken?> GetByRealmIdAsync(string realmId)
        {
            if (string.IsNullOrWhiteSpace(realmId))
            {
                return null;
            }
            return await _context.Set<QuickBooksToken>().FirstOrDefaultAsync(x => x.RealmId == realmId);
        }

        public async Task<Response> AddOrUpdateAsync(QuickBooksToken quickBooksToken)
        {
            if (quickBooksToken == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "QuickBooks token is required."
                };
            }

            if (string.IsNullOrWhiteSpace(quickBooksToken.RealmId))
            {
                return new Response
                {
                    Status = false,
                    Message = "QuickBooks RealmId is required."
                };
            }
            var existingToken = await GetByRealmIdAsync(quickBooksToken.RealmId);
            if (existingToken == null)
            {
                await AddAsync(quickBooksToken);
            }
            else
            {
                existingToken.AccessToken =quickBooksToken.AccessToken;
                existingToken.RefreshToken =quickBooksToken.RefreshToken;
                existingToken.TokenType =quickBooksToken.TokenType;
                existingToken.AccessTokenExpiresAt =quickBooksToken.AccessTokenExpiresAt;
                existingToken.RefreshTokenExpiresAt =quickBooksToken.RefreshTokenExpiresAt;
                existingToken.EditedDate =DateTime.UtcNow;
            }
            return new Response
            {
                Status = true,
                Message = existingToken == null
                    ? "QuickBooks token created successfully."
                    : "QuickBooks token updated successfully.",

                ReturnResponse =
                    existingToken ?? quickBooksToken
            };
        }
    }
}

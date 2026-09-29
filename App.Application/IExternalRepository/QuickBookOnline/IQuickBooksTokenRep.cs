using App.Application.DTOs.QuickBookOnlineDTO;
using App.Application.DTOs.Sys_DTO;
using App.Common.GenericResponse;
using App.Domain.Entities.QuickBooksOnline;
using App.Domain.Entities.Sec_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IExternalRepository.QuickBookOnline
{
    public interface IQuickBooksTokenRep
    {
        /// <summary>
        /// This method is used to create a new QuickBooks token in the database.
        /// </summary>
        /// <param name="quickBooksToken"></param>
        /// <returns></returns>
        Task<Response> CreateAsync(QuickBooksToken quickBooksToken);

        /// <summary>
        /// This method is used to update an existing QuickBooks token in the database.
        /// </summary>
        /// <param name="quickBooksToken"></param>
        /// <returns></returns>
        Task<Response> UpdateTokenAsync(QuickBooksToken quickBooksToken);

        /// <summary>
        /// This method is used to get a QuickBooks token by realmId from the database.
        /// </summary>
        /// <param name="realmId"></param>
        /// <returns></returns>
        Task<QuickBooksToken> GetByRealmIdAsync(string realmId);

        Task<Response> AddOrUpdateAsync(QuickBooksToken quickBooksToken);
    }
}

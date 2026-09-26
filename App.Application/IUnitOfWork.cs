using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }

}

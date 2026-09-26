using System;
using System.Collections.Generic;
using System.Text;
using AapRepository;
using App.Application;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DB_Contexts _context;

        public UnitOfWork(DB_Contexts context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database
                    .CurrentTransaction
                    .CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database
                    .CurrentTransaction
                    .RollbackAsync();
            }
        }
    }


}

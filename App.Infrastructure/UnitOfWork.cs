using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AapRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DB_Contexts _context;

        public UnitOfWork(DB_Contexts context)
        {
            _context = context;
            //_UserRep = new UserRep(_context); // Can use DI here if preferred
        }

        //public IUserRep _UserRep { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

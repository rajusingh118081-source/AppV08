using App.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AapRepository
{
    public class DB_Contexts : DbContext
    {
        public DB_Contexts(DbContextOptions<DB_Contexts> options)
        : base(options)
        {
        }

        public DbSet<Sec_Users> Sec_Users { get; set; }
    }
}

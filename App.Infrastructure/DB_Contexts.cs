
using App.Domain.Entities.Ref_Model;
using App.Domain.Entities.Sec_Model;
using Microsoft.EntityFrameworkCore;

namespace AapRepository
{
    public class DB_Contexts : DbContext
    {
        public DB_Contexts(DbContextOptions<DB_Contexts> options)
        : base(options)
        {
        }

        public DbSet<Sec_Users> Sec_Users { get; set; }
        public DbSet<Ref_SysDataManagerFields> Ref_SysDataManagerFields { get; set; }
    }
}

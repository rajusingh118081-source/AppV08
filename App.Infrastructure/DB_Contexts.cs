
using App.Domain.Entities.Main_Model;
using App.Domain.Entities.QuickBooksOnline;
using App.Domain.Entities.Ref_Model;
using App.Domain.Entities.Sec_Model;
using Intuit.Ipp.Data;
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
        public DbSet<QuickBooksToken> QuickBooksToken { get; set; }
        public DbSet<Main_Invoices> Main_Invoices { get; set; }
        public DbSet<Main_InvoiceLineItems> Main_InvoiceLineItems { get; set; }
        public DbSet<Main_Contacts> Main_Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Main_Invoices>()
                .HasIndex(x => x.QboInvoiceID)
                .IsUnique();

            modelBuilder.Entity<Main_Invoices>()
                .HasMany(x => x.LineItems)
                .WithOne(x => x.Invoice)
                .HasForeignKey(x => x.LinkedInvoiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

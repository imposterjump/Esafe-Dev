using BankProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions <AppDbContext>options) : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Admin> Admins { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasOne(e => e.Client)
                .WithMany(e => e.ClientAddresses)
                .HasForeignKey(e => e.ClientID);
        }
        
    }
}

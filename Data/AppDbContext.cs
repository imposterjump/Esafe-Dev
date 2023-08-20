using BankProject.Models;
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
        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasOne(e => e.Client)
                .WithMany(e => e.Addresses)
                .HasForeignKey(e => e.ClientID);
        }

        
    }
}

using BugAndFix_Concurrency_Sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace BugAndFix_Concurrency_Sample.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Shipment>? Shipments { get; set; }

    public virtual DbSet<Package> Packages { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Package>()
            .Property(p => p.RowVersion).IsConcurrencyToken();
    }



}

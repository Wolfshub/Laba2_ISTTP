using Laba_ISTP_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Laba_ISTP_2.Context;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Library>()
           .HasMany(l => l.Books)
           .WithOne(b => b.Library)
           .OnDelete(DeleteBehavior.Cascade);
    }
    public DbSet<Book> Books { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Member> Members { get; set; }
}

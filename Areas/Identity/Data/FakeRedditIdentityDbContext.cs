using FakeReddit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FakeReddit.Areas.Identity.Data;

public class FakeRedditIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public FakeRedditIdentityDbContext(DbContextOptions<FakeRedditIdentityDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Theme> Theme { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        /*builder.Entity<ApplicationUser>(b =>
        {
            b.HasMany(e => e.Themes)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();
        });*/
    }
}

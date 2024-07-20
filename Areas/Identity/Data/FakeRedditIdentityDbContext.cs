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
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Rate> Rate { get; set; }
    
}

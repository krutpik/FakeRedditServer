using Microsoft.EntityFrameworkCore;
using FakeReddit.Models;

namespace MvcMovie.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext (DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<Paper> Paper { get; set; }
    }
}
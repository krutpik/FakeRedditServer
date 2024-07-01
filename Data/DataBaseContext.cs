using FakeReddit.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeReddit.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext (DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }
    }
}
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharlieBackend.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        public DbSet<Sample> Samples { get; set; }
    }
}

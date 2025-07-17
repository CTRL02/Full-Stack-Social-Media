using Microsoft.EntityFrameworkCore;

namespace socialmedia.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Entities.AppUser> Users { get; set; }
 
    }
}

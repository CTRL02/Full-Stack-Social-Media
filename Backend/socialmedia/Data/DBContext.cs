using Microsoft.EntityFrameworkCore;
using socialmedia.DTOs;
using socialmedia.Entities;

namespace socialmedia.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFollowStatsDto>().HasNoKey();
            modelBuilder.Entity<UserPostCountDto>().HasNoKey();

            modelBuilder.Entity<UserFollow>()
                .HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FolloweeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<Post>()
                .HasIndex(p => p.AppUserId);

            modelBuilder.Entity<SocialLink>()
                .HasIndex(s => s.AppUserId);

            modelBuilder.Entity<UserFollow>()
                .HasIndex(uf => uf.FolloweeId);

            modelBuilder.Entity<UserFollow>()
                .HasIndex(uf => uf.FollowerId);
        }

        public DbSet<UserFollowStatsDto> UserFollowStats { get; set; } 

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<UserPostCountDto> UserPostCounts { get; set; }  


    }
}

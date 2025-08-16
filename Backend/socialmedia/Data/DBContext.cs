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
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<Impression>()
                .HasOne(i => i.Post)
                .WithMany(p => p.Impressions)
                .HasForeignKey(i => i.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Impression>()
                .HasOne(i => i.Comment)
                .WithMany(c => c.Impressions)
                .HasForeignKey(i => i.CommentId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Impression>()
                .HasOne(i => i.AppUser)
                .WithMany(u => u.Impressions)
                .HasForeignKey(i => i.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);


            //messaging
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);


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


            // Ensure a user can only leave one impression per post
            modelBuilder.Entity<Impression>()
                .HasIndex(i => new { i.AppUserId, i.PostId })
                .IsUnique()
                .HasFilter("[PostId] IS NOT NULL");

            // Ensure a user can only leave one impression per comment
            modelBuilder.Entity<Impression>()
                .HasIndex(i => new { i.AppUserId, i.CommentId })
                .IsUnique()
                .HasFilter("[CommentId] IS NOT NULL");

        }



        public DbSet<UserFollowStatsDto> UserFollowStats { get; set; } 

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<UserPostCountDto> UserPostCounts { get; set; }

        // both are not needed as i am using collection inside posts and appuser entities but added for flexibilty -- can be removed --
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Impression> Impressions { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}

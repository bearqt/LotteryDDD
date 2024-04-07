using LotteryDDD.Domain.Aggregates;
using LotteryDDD.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LotteryDDD.Infrastructure.Data
{
    public class EfDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameUser> GameUsers { get; set; }

        public EfDbContext()
        {
        }

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(r => r.Id).ValueGeneratedNever()
                .HasConversion<Guid>(userId => userId.Value, dbId => UserId.Of(dbId));

            modelBuilder.Entity<User>().OwnsOne(x => x.Username,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(Username))
                    .IsRequired();
                });
            modelBuilder.Entity<User>().OwnsOne(x => x.Balance,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(Balance))
                    .IsRequired();
                });

            modelBuilder.Entity<Game>().HasKey(x => x.Id);
            modelBuilder.Entity<Game>().Property(r => r.Id).ValueGeneratedNever()
                .HasConversion<Guid>(gameId => gameId.Value, dbId => GameId.Of(dbId));

            modelBuilder.Entity<Game>().OwnsOne(x => x.BetAmount,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(BetAmount))
                    .IsRequired();
                });

            modelBuilder.Entity<Game>()
                .HasMany(x => x.Users)
                .WithOne()
                .HasForeignKey("GameId");


            modelBuilder.Entity<GameUser>().HasKey(x => new {x.GameId, x.UserId });
            modelBuilder.Entity<GameUser>().Property(x => x.GameId)
                .HasConversion<Guid>(gameId => gameId.Value, dbId => GameId.Of(dbId));
            modelBuilder.Entity<GameUser>().Property(x => x.UserId)
                .HasConversion<Guid>(userId => userId.Value, dbId => UserId.Of(dbId));
        }
    }
}

using LotteryDDD.Domain.Aggregates;
using LotteryDDD.Domain.Common;
using LotteryDDD.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LotteryDDD.Infrastructure.Data
{
    public class EfDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameUser> GameUsers { get; set; }
        public DbSet<GameNumber> GameNumbers { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Report> Reports { get; set; }

        private IMediator _mediator;

        public EfDbContext()
        {
        }

        public EfDbContext(DbContextOptions<EfDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(r => r.Id).ValueGeneratedNever()
                .HasConversion<Guid>(userId => userId.Value, dbId => UserId.Of(dbId));

            modelBuilder.Entity<User>().ComplexProperty(x => x.Username,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(Username))
                    .IsRequired();
                });
            modelBuilder.Entity<User>().ComplexProperty(x => x.Balance,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(Balance))
                    .IsRequired();
                });

            modelBuilder.Entity<Game>().HasKey(x => x.Id);
            modelBuilder.Entity<Game>().Property(r => r.Id).ValueGeneratedNever()
                .HasConversion<Guid>(gameId => gameId.Value, dbId => GameId.Of(dbId));
            
            modelBuilder.Entity<Game>().ComplexProperty(x => x.BetAmount,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(Game.BetAmount))
                    .IsRequired();
                });

            modelBuilder.Entity<Game>().ComplexProperty(x => x.RoundNumber,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(Game.RoundNumber))
                    .IsRequired();
                });

            modelBuilder.Entity<Game>()
                .HasMany(x => x.Users)
                .WithOne()
                .HasForeignKey(x => x.GameId);

            modelBuilder.Entity<Game>()
                .HasMany(x => x.Numbers)
                .WithOne()
                .HasForeignKey(x => x.GameId);


            modelBuilder.Entity<GameUser>().HasKey(x => new {x.GameId, x.UserId });
            modelBuilder.Entity<GameUser>().Property(x => x.GameId)
                .HasConversion<Guid>(gameId => gameId.Value, dbId => GameId.Of(dbId));
            modelBuilder.Entity<GameUser>().Property(x => x.UserId)
                .HasConversion<Guid>(userId => userId.Value, dbId => UserId.Of(dbId));

            modelBuilder.Entity<GameNumber>().HasKey(x => x.Id);
            modelBuilder.Entity<GameNumber>().Property(r => r.Id).ValueGeneratedNever()
                .HasConversion<Guid>(userId => userId.Value, dbId => GameNumberId.Of(dbId));

            modelBuilder.Entity<GameNumber>().Property(x => x.GameId)
                .HasConversion<Guid>(gameId => gameId.Value, dbId => GameId.Of(dbId));
            modelBuilder.Entity<GameNumber>().ComplexProperty(x => x.NumberValue,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(GameNumber.NumberValue))
                    .IsRequired();
                });

            modelBuilder.Entity<Score>().HasKey(x => new { x.GameId, x.UserId });
            modelBuilder.Entity<Score>().Property(x => x.GameId)
                .HasConversion<Guid>(gameId => gameId.Value, dbId => GameId.Of(dbId));
            modelBuilder.Entity<Score>().Property(x => x.UserId)
                .HasConversion<Guid>(userId => userId.Value, dbId => UserId.Of(dbId));
            modelBuilder.Entity<Score>().ComplexProperty(x => x.Points,
                a =>
                {
                    a.Property(p => p.Value)
                    .HasColumnName(nameof(Score.Points))
                    .IsRequired();
                });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            // Dispatch Domain Events collection.
            await DispatchEvents(cancellationToken);

            return result;
        }

        private async Task DispatchEvents(CancellationToken cancellationToken)
        {
            var domainEntities = ChangeTracker
                .Entries<IAggregate>()
                .Where(x => x.Entity.GetDomainEvents() != null && x.Entity.GetDomainEvents().Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents())
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}

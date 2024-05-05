using LotteryDDD.Application;
using LotteryDDD.Domain.Aggregates.Events;
using LotteryDDD.Infrastructure.Data;
using MediatR;

namespace LotteryDDD.Handlers
{
    public class GameStartedDomainEventHandler : INotificationHandler<GameStartedDomainEvent>
    {
        private readonly IUsersNotifierService _notifier;
        private readonly EfDbContext _dbContext;
        public GameStartedDomainEventHandler(IUsersNotifierService notifier, EfDbContext dbContext)
        {
            _notifier = notifier;
            _dbContext = dbContext;
        }

        public Task Handle(GameStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            var users = _dbContext.GameUsers
                .Where(x => x.GameId == notification.id)
                .Select(x => x.UserId)
                .ToList();

            foreach (var userId in users) {
                _notifier.NotifyUser(userId.Value, "The game has started!");
            }

            return Task.CompletedTask;
        }
    }
}

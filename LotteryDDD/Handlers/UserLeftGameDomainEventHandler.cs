﻿using LotteryDDD.Application;
using LotteryDDD.Domain.Aggregates;
using LotteryDDD.Domain.Aggregates.Events;
using LotteryDDD.Domain.ValueObjects;
using LotteryDDD.Infrastructure.Data;
using MediatR;
using System.Text.Json;

namespace LotteryDDD.Handlers
{
    public class UserLeftGameReportDomainEventHandler : INotificationHandler<UserLeftGameDomainEvent>
    {
        private readonly EfDbContext _dbContext;
        private readonly KafkaProducerService _kafkaProducerService;
        public UserLeftGameReportDomainEventHandler(EfDbContext dbContext, KafkaProducerService kafkaProducerService)
        {
            _dbContext = dbContext;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task Handle(UserLeftGameDomainEvent notification, CancellationToken cancellationToken)
        {
            var message = new UserLeftGameMessage(Guid.NewGuid(), notification.gameId, notification.userId, notification.username, new DateTime().ToString());
            await _kafkaProducerService.ProduceAsync("userLeftGame", JsonSerializer.Serialize(message));
            // Репортим ливера!
            var report = Report.Create(ReportId.Of(Guid.NewGuid()), 
                UserId.Of(notification.userId),
                "The user has left the game prematurely ");

            _dbContext.Reports.Add(report);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class UserLeftGameNotifyDomainEventHandler : INotificationHandler<UserLeftGameDomainEvent>
    {
        private readonly IUsersNotifierService _notifier;
        private readonly EfDbContext _dbContext;

        public UserLeftGameNotifyDomainEventHandler(IUsersNotifierService notifier, EfDbContext dbContext)
        {
            _notifier = notifier;
            _dbContext = dbContext;
        }

        public async Task Handle(UserLeftGameDomainEvent notification, CancellationToken cancellationToken)
        {
            var users = _dbContext.GameUsers
                .Where(x => x.GameId == notification.gameId)
                .Select(x => x.UserId)
                .ToList();

            foreach (var userId in users)
            {
                _notifier.NotifyUser(userId.Value, "The game has restarted, due to user left the game :(");
            }

            var game = _dbContext.Games.FirstOrDefault(x => x.Id == notification.gameId);
            game.Restart();
            await _dbContext.SaveChangesAsync();
        }
    }
}

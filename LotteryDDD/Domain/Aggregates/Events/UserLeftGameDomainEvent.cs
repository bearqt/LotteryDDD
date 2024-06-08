using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Aggregates.Events
{
    public record UserLeftGameDomainEvent
        (Guid gameId, Guid userId, string username) : IDomainEvent
    {
    }
    public record UserLeftGameMessage
        (Guid id, Guid gameId, Guid userId, string username, string timestamp);
}

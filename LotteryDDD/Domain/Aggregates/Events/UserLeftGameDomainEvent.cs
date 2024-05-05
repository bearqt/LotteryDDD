using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Aggregates.Events
{
    public record UserLeftGameDomainEvent
        (Guid gameId, Guid userId) : IDomainEvent
    {
    }
}
